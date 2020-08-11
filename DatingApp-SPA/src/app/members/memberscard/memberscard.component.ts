import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-memberscard',
  templateUrl: './memberscard.component.html',
  styleUrls: ['./memberscard.component.css']
})
export class MemberscardComponent implements OnInit {

  @Input() user : User;
  constructor() { }

  ngOnInit() {
  }

}
