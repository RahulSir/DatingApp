import { UserService } from './../../_Service/user.service';
import { AlertifyService } from './../../_Service/alertify.service';
import { AuthService } from './../../_Service/auth.service';

import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/_models/Message';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
})
export class MemberMessagesComponent implements OnInit {
  @Input() recepientId: number;
  messages: Message[] = [];
  newMessage: any = {};
  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.loadMessages();
  }
  loadMessages() {
    const currentUserId = +this.authService.decodedToken.nameid;
    this.userService
      .getMessageThread(currentUserId, this.recepientId)
      // tap allows to do stuff before subscribing to any obserable
      .pipe(
        tap((message) => {
          for (let i = 0; i < message.length; i++) {
            if (
              message[i].isRead === false &&
              message[i].recepientId === currentUserId
            ) {
              this.userService.markAsRead(message[i].id, currentUserId);
            }
          }
        })
      )
      .subscribe(
        (messages) => {
          this.messages = messages;
        },
        (error) => {
          this.alertify.error(error);
        }
      );
  }
  sendMessage() {
    this.newMessage.recepientId = this.recepientId;
    this.userService
      .sendMessage(this.authService.decodedToken.nameid, this.newMessage)
      .subscribe(
        (message: Message) => {
          this.messages.unshift(message);
          this.newMessage.content = '';
        },
        (error) => {
          this.alertify.error(error);
        }
      );
  }
}
