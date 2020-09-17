import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { UserService } from './../_Service/user.service';
import { User } from 'src/app/_models/user';
import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_Service/alertify.service';

@Injectable()
export class ListsResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private router: Router
  ) {}

  pageNumber = 1;
  itemsPerPage = 5;
  likesParams = 'Likers';
  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    return this.userService
      .getUsers(this.pageNumber, this.itemsPerPage, null, this.likesParams)
      .pipe(
        catchError((error) => {
          this.alertify.error('Problem retreiving data');
          this.router.navigate(['/home']);
          return of(null);
        })
      );
  }
}
