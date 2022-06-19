import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(public router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error) => {
        if (error) {
          switch (error.status){
            case 400:
              if(error.error.errors) {
                const modalStateErrors = [];
                for (const key in error.error.errors){
                  if(error.error.errors[key]){
                    modalStateErrors.push(error.error.errors[key])
                  }
                  throw modalStateErrors.flat();
                }
              }
              else {                
                this.toastr.error('Bad request', error.status);
              }
              break;
            case 401:
              this.toastr.error('User not authorized', error.status);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');  
              this.toastr.error('Page not found', error.status);                           
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state: {error: error.error}};
              this.router.navigateByUrl('/server-error', navigationExtras);
              this.toastr.error('Internal server error', error.status);                            
              break;
            default:
              this.toastr.error('Something unexpected happened');
              console.log(error);
              break;
          }
        }
        return throwError(() => new Error('Intercepted error'));
      })
    );
  }
}
