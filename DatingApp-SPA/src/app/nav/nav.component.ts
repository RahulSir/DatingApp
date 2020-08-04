import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_Service/auth.service';
import { nextTick } from 'process';
import { AlertifyService } from '../_Service/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(
    public authService: AuthService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {}

  login() {
    this.authService.login(this.model).subscribe(
      (next) => {
        this.alertify.success('Logged in succesfully');
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }

  loggedIn() {
    return this.authService.loggedIn();
    // const token = localStorage.getItem('token');
    // // below statement is short hand for if null return false else return true
    // return !!token;
  }

  logOut() {
    localStorage.removeItem('token');
    this.alertify.success("Logged Out successfully");
  }
}
