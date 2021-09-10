import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatMenuTrigger } from '@angular/material/menu';
import { Role } from 'src/app/users/models/role';
import { User } from 'src/app/users/models/user';
import { UserDataService } from 'src/app/users/services/user-data.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
})
export class UserProfileComponent implements OnInit{

  @ViewChild('menuTrigger') menuTrigger!: MatMenuTrigger;
  @Input() user!:User;
  avatarUrl:String | undefined;


  constructor(public dialog: MatDialog,
    public notificService: NotificationService,
    public userService: UserDataService) {
  }

  ngOnInit(){
    this.userService.getById(this.user.id!).subscribe(
      response => {
        this.user = response;
        this.avatarUrl = (this.user.avatarUrl ?? '') +'?'+ performance.now();
      });
    
  }

  getMainRoles(){
    if(this.user.roles?.filter(x=>x.name=='HrLead').length != 0){
      return this.user.roles?.filter(x=>x.name!='HrUser');
    }
    if(this.user.roles == null || this.user.roles == undefined || this.user.roles.length == 0){
      let newSimpleRole:Role = {
        key: 1,
        name: 'HrUser',
      };
      this.user.roles.push(newSimpleRole);
    }
      
    return this.user.roles;
  }

  public notifyPhone(payload: string) {
    this.notificService.showInfoMessage('The phone has been copied to clipboard');
  }
  public notifySkype(payload: string) {
    this.notificService.showInfoMessage('The skype link has been copied to clipboard');
  }
  
}

