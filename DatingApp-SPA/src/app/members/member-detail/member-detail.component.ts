import { User } from 'src/app/_models/user';
import { UserService } from './../../_Service/user.service';
import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_Service/alertify.service';
import { ActivatedRoute } from '@angular/router';
import {
  NgxGalleryOptions,
  NgxGalleryImage,
  NgxGalleryAnimation,
} from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}
  user: User;
  ngOnInit() {
    this.route.data.subscribe(data =>{
      this.user = data['user'];
    })

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      },
    ];
    this.galleryImages = this.getImages();
  }



  getImages() {
    const imageUrls = [];


    for (const image of this.user.photos) {
      imageUrls.push({
        small: image.url,
        big: image.url,
        medium: image.url,
        desription: image.description,
      });
    }
    return imageUrls;
  }
}
