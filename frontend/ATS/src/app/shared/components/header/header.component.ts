import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import _ from 'lodash';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/users/services/auth.service';
import { EditVacancyComponent } 
  from 'src/app/vacancies/components/edit-vacancy/edit-vacancy.component';
import { NotificationService } from '../../services/notification.service';
import { User } from 'src/app/users/models/user';
import { UserDataService } from 'src/app/users/services/user-data.service';
import { EditHrFormComponent } from 'src/app/users/components/edit-hr-form/edit-hr-form.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit, OnDestroy {
  public value: string = '';
  public dropdownOpened: boolean = false;
  @Input() removeButton = false;
  user:User = {} as User;
  ngOnInit(){
    this.userService.getByToken().subscribe(
      response => {
        this.user = response;
        if(this.user.avatarUrl) this.user.avatarUrl = this.user.avatarUrl + '?'+performance.now();
      });
  }

  public loading: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly service: AuthenticationService,
    private readonly dialog: MatDialog,
    private readonly notifications: NotificationService,
    private readonly userService: UserDataService,

  ) {}

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }


  openDialog(): void {
    const dialogRef = this.dialog.open(EditVacancyComponent, {
      width: '600px',
      height: 'auto',
      data: {},
    });
  };
  

  public logout(): void {
    this.loading = true;

    this.service
      .logout()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        () => {
          this.loading = false;
          window.location.replace('/login');
        },
        () => {
          this.loading = false;
          window.location.replace('/login');
        },
      );
  }


  onOpenProfile(){
    const dialogRef = this.dialog.open(EditHrFormComponent, {
      width: '70%',
      height: 'auto',
      data: {userToEdit:this.user},
    }).afterClosed()
      .subscribe(() => this.refreshUser());
  }

  refreshUser(){
    this.userService.getByToken().subscribe(
      response => {
        this.user = response;
        if(this.user.avatarUrl) this.user.avatarUrl = this.user.avatarUrl + '?'+performance.now();

      });
  }

}
