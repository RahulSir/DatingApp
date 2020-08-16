import { User } from './_models/user';
import { Component, OnInit } from '@angular/core';
import { AuthService } from './_Service/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  jwthelper = new JwtHelperService();

  constructor(private authService: AuthService) {}
  ngOnInit() {
    const token = localStorage.getItem('token');
    const user: User = JSON.parse(localStorage.getItem('user'));
    if (token) {
      this.authService.decodedToken = this.jwthelper.decodeToken(token);
    }
    if (user) {
      console.log(user)
      this.authService.currentUser = user;
      this.authService.changeMemberPhoto(user.photourl);
    }
  }
}
