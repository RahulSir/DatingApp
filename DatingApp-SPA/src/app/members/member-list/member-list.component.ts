import { ActivatedRoute } from '@angular/router';
import { UserService } from './../../_Service/user.service';
import { User } from './../../_models/user';
import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../../_Service/alertify.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  users: User[];

  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.users = data['users'];
    });
  }
}
