import { catchError } from 'rxjs/operators';
import { Observable ,of} from 'rxjs';
import { UserService } from './../_Service/user.service';
import { User } from 'src/app/_models/user';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRoute, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_Service/alertify.service';

@Injectable()
export class MemberDetailResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private router: Router
  ){}

  resolve(route: ActivatedRouteSnapshot): Observable<User>{
    return this.userService.getUser(route.params['id']).pipe(
      catchError(error => {
        this.alertify.error("Problem retreiving data");
        this.router.navigate(['/members']);
        return of(null);

      })
    )
  }
}
