import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AlertifyService } from '../_Service/alertify.service';
import { AuthService } from '../_Service/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthguardGuard implements CanActivate {
  constructor(
    private route: Router,
    private alertify: AlertifyService,
    private authservice: AuthService
  ) {}
  canActivate(): boolean {
    if (this.authservice.loggedIn()) {
      return true;
    }
    else{
      this.alertify.warning('You can not pass !!!!!!!');
      this.route.navigate(['/home']);
      return false;
    }
  }
}
