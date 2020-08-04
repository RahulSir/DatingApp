import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_Service/auth.service';
import { CommentStmt } from '@angular/compiler';
import { AlertifyService } from '../_Service/alertify.service';

@Component({
  selector: 'app-Register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css'],
})
export class RegisterComponent implements OnInit {

  @Output() CancelRegister = new EventEmitter();
  model: any = {};
  constructor(private authService: AuthService,
    private alertify : AlertifyService) {}

  ngOnInit() {}

  register() {
    this.authService.register(this.model).subscribe(
      () => {
        this.alertify.success("Registered Succesfully");
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }

  Cancel() {
    this.CancelRegister.emit(false);
    console.log('Cancelled');
  }
}
