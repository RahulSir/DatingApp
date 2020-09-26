import { Message } from './../_models/Message';
import { AuthService } from './../_Service/auth.service';
import { PaginatedResult, Pagination } from './../_models/Pagination';
import { AlertifyService } from './../_Service/alertify.service';
import { UserService } from './../_Service/user.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  messageContainer = 'Unread';
  pageNumber = 1;
  itemsPerPage = 5;
  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private alertify: AlertifyService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.messages = data['messages'].result;
      this.pagination = data['messages'].pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }

  loadMessages() {

    this.userService
      .getMessages(
        this.authService.decodedToken.nameid,
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.messageContainer
      )
      .subscribe(
        (res: PaginatedResult<Message[]>) => {
          this.messages = res.result;
          this.pagination = res.pagination;
        },
        (error) => {
          this.alertify.error(error);
        }
      );
  }
  deleteMessage(id: number) {
    this.alertify.confirm(
      'Are you sure you want to delete the message?',
      () => {
        this.userService
          .deleteMessage(id, this.authService.decodedToken.nameid)
          .subscribe(
            () => {
              this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
              this.alertify.success('Successfully deleted');
            },
            (error) => {
              this.alertify.error(error);
            }
          );
      }
    );
  }
}
