import { AuthService } from './../_Service/auth.service';
import { Message } from './../_models/Message';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { UserService } from '../_Service/user.service';
import { User } from 'src/app/_models/user';
import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_Service/alertify.service';

@Injectable()
export class MessagesResolver implements Resolve<Message[]> {
  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private router: Router,
    private authService: AuthService
  ) {}

  pageNumber = 1;
  itemsPerPage = 5;
  messagesContainer = 'UnRead';
  resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
    return this.userService.getMessages(this.authService.decodedToken.nameid,this.pageNumber, this.itemsPerPage,
      this.messagesContainer).pipe(
      catchError((error) => {
        this.alertify.error('Problem retreiving messages');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
