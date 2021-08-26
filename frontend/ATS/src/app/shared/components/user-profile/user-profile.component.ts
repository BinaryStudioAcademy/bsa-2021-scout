import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatMenuTrigger } from '@angular/material/menu';
import { User } from 'src/app/users/models/user';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
})
export class UserProfileComponent {

  @ViewChild('menuTrigger') menuTrigger!: MatMenuTrigger;
  @Input() user!:User;


  constructor(public dialog: MatDialog, public notificService: NotificationService) {
  }
  public notifyPhone(payload: string) {
    this.notificService.showInfoMessage('The phone has been copied to clipboard');
  }
  public notifySkype(payload: string) {
    this.notificService.showInfoMessage('The skype link has been copied to clipboard');
  }
  
}

