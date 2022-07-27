import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();

  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];

  constructor(private accountService: AccountService,
              private toastr: ToastrService,
              private fb: FormBuilder,
              private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', [Validators.required, Validators.minLength(4)]],
      knownAs: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      city: ['', [Validators.required]],
      country: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
  }

  matchValues(matchTo: string) : ValidatorFn | null { // Проверка, confirmPassword = password с помощью кастомного валидатора
      return (control: AbstractControl) => { 
      // return control.value === control?.parent?.controls[matchTo].value ? null : {isMatching: true}

      const controls = control?.parent?.controls as { [key: string]: AbstractControl; };
        return controls && control?.value === controls[matchTo]?.value ? null : {isMatching: true}
    }
  }

  // passwordsMatch: ValidatorFn = (group: AbstractControl):  ValidationErrors | null => { 
  //   let pass = group.get('password').value;
  //   let confirmPass = group.get('confirmPassword').value
  //   return pass === confirmPass ? null : { notSame: true }
  // }

  register() {
    this.accountService.register(this.registerForm.value).subscribe({
        next: (response) => {
          this.router.navigateByUrl('/members');
        },
        error: (error) => {
          this.validationErrors = error;
        }
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
