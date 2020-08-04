import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';
import { ok } from 'assert';

@Injectable({
  providedIn: 'root',
})
export class AlertifyService {
  constructor() {}

  confirm(message: string, okcallback: () => any) {
    alertify.confirm(message, (e: any) => {
      if (e) {
        okcallback();
      } else {
      }
    });
  }

  success(message : string){
    alertify.success(message);
  }
  error(message : string){
    alertify.error(message);
  }
  warning(message : string){
    alertify.warning(message);
  }
  message(message : string){
    alertify.message(message);
  }

}
