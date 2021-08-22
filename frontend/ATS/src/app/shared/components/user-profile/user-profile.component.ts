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
      image: "https://images.pexels.com/photos/733872/pexels-photo-733872.jpeg?cs=srgb&dl=pexels-andrea-piacquadio-733872.jpg&fm=jpg",
      skype: "@string",
      phone:"+380123456789",
      firstName: "Hanna",
      lastName: "Roberts",
      roles: [{
        name: "HR Lead",
        key: 1
      }]
    }
  }
  copyMessage(val: string){
    const selBox = document.createElement('textarea');
    selBox.style.position = 'fixed';
    selBox.style.left = '0';
    selBox.style.top = '0';
    selBox.style.opacity = '0';
    selBox.value = val;
    document.body.appendChild(selBox);
    selBox.focus();
    selBox.select();
    document.execCommand('copy');
    document.body.removeChild(selBox);
  }
  
}
