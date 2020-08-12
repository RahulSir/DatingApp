import { AuthService } from './../_Service/auth.service';
import { catchError } from 'rxjs/operators';
import { Observable ,of} from 'rxjs';
import { UserService } from '../_Service/user.service';
import { User } from 'src/app/_models/user';
import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_Service/alertify.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private router: Router,
    private authService: AuthService
  ){}

  resolve(route: ActivatedRouteSnapshot): Observable<User>{
    return this.userService.getUser(this.authService.decodedToken.nameid).pipe(
      catchError(error => {
        this.alertify.error("Problem retreiving your data");
        this.router.navigate(['/home']);
        return of(null);

      })
    )
  }
}
