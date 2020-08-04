import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-Home',
  templateUrl: './Home.component.html',
  styleUrls: ['./Home.component.css'],
})
export class HomeComponent implements OnInit {
  registerMode:boolean = false;

  constructor(private http: HttpClient) {}

  ngOnInit() {

  }

  register(){
    this.registerMode = true ;
  }

  CancelRegisterMode(registermode : boolean){
    this.registerMode = registermode ;
  }
}




