import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatMenuTrigger } from '@angular/material/menu';
import { User } from 'src/app/users/models/user';
import { UserProfile } from '../../models/users/user-profile';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent {

  @ViewChild('menuTrigger') menuTrigger!: MatMenuTrigger;
  @Input() user!:UserProfile;


  constructor(public dialog: MatDialog) {
    
    this.user = {
      image: "string.jpg",
      skype: "@string",
      phone:"+380123456789",
      firstName: "Irina",
      lastName: "K",
      roles: [{
        name: "HR",
        key: 1
      }]
    }
  }

  
}
