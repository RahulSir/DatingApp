import { error } from 'protractor';
import { UserService } from './../../_Service/user.service';
import { AuthService } from './../../_Service/auth.service';
import { AlertifyService } from './../../_Service/alertify.service';
import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-memberscard',
  templateUrl: './memberscard.component.html',
  styleUrls: ['./memberscard.component.css'],
})
export class MemberscardComponent implements OnInit {
  @Input() user: User;

  constructor(
    private alertify: AlertifyService,
    private authService: AuthService,
    private userSevice: UserService
  ) {}

  ngOnInit() {}

  sendLike(recepiendId: number) {
    this.userSevice
      .sendLike(this.authService.decodedToken.nameid, recepiendId)
      .subscribe(
        (data) => {
          this.alertify.message('You have liked ' + this.user.knownAs);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
  }
}
