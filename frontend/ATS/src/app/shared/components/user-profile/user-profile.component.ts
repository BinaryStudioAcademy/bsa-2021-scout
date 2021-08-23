import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatMenuTrigger } from '@angular/material/menu';
import { User } from 'src/app/users/models/user';
import { UserProfile } from '../../models/users/user-profile';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent {

  @ViewChild('menuTrigger') menuTrigger!: MatMenuTrigger;
  @Input() user!:UserProfile;


  constructor(public dialog: MatDialog, public notificService: NotificationService) {
    
    this.user = {
      image: "https://images.pexels.com/photos/733872/pexels-photo-733872.jpeg?cs=srgb&dl=pexels-andrea-piacquadio-733872.jpg&fm=jpg",
      skype: "@h.roberts",
      phone:"+380123456789",
      firstName: "Hanna",
      lastName: "Roberts",
      roles: [{
        name: "HR Lead",
        key: 1
      }]
    }
  }
  public notifyPhone(payload: string) {
    this.notificService.showInfoMessage(`The phone has been copied to clipboard`);
}
public notifySkype(payload: string) {
  this.notificService.showInfoMessage(`The skype link has been copied to clipboard`);
}
  
}

