import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_Service/auth.service';
import { CommentStmt } from '@angular/compiler';

@Component({
  selector: 'app-Register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css'],
})
export class RegisterComponent implements OnInit {

  @Output() CancelRegister = new EventEmitter();
  model: any = {};
  constructor(private authService: AuthService) {}

  ngOnInit() {}

  register() {
    this.authService.register(this.model).subscribe(
      () => {
        console.log('Registered Successfully');
      },
      (error) => {
        console.log(error);
      }
    );
  }

  Cancel() {
    this.CancelRegister.emit(false);
    console.log('Cancelled');
  }
}
