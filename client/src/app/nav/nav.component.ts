import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}

  constructor(public accountService: AccountService, 
              private router: Router) { }

  ngOnInit(): void {
  }

  // Del MAS 03.06.2022
  // Old style login method
  /*   login(){
      this.accountService.login(this.model).subscribe(response => {
        console.log(response);
        this.loggedIn = true;
      }, error => {
        console.log(error)
      })
   */

  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigateByUrl('/members');
      },
      error: (error) => {
         console.log(error);
        }
    })
  };

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
