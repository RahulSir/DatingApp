import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_Service/auth.service';
import { nextTick } from 'process';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService) {}

  ngOnInit() {}

  login(){
    this.authService.login(this.model).subscribe(next => {
      console.log("Logged in");
    } , error => {
      console.log(error);
    }
    );

  }

  loggedIn() {
    const token = localStorage.getItem('token');
    // below statement is short hand for if null return false else return true
    return !!token;
  }

  logOut() {
    localStorage.removeItem('token');
    console.log('Logged Out successfully');
  }
}
