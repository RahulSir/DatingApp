import { Router } from '@angular/router';
import { User } from './../_models/user';
import {
  BsDatepickerModule,
  BsDatepickerConfig,
} from 'ngx-bootstrap/datepicker';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_Service/auth.service';
import { CommentStmt } from '@angular/compiler';
import { AlertifyService } from '../_Service/alertify.service';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { datepickerAnimation } from 'ngx-bootstrap/datepicker/datepicker-animations';

@Component({
  selector: 'app-Register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() CancelRegister = new EventEmitter();
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;
  user: User;
  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red',
      // minDate: new Date('09/12/2020')
    };
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group(
      {
        gender: ['male', Validators.required],
        username: ['', Validators.required],
        knownAs: ['', Validators.required],
        dateOfBirth: [null, Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(20),
          ],
        ],

        confirmpassword: ['', Validators.required],
      },
      { validator: this.checkpasswordmatch }
    );
  }

  checkpasswordmatch(g: FormGroup) {
    return g.get('password').value === g.get('confirmpassword').value
      ? null
      : { mismatch: true };
  }
  register() {
    this.user = this.registerForm.value;
    this.authService.register(this.user).subscribe(
      () => {
        this.alertify.success('Registered Succesfully');
      },
      (error) => {
        this.alertify.error(error);
      },
      () => {
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/members']);
        });
      }
    );
  }

  Cancel() {
    this.CancelRegister.emit(false);
    console.log('Cancelled');
  }
}
