import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnDestroy } from '@angular/core';
import _ from 'lodash';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/users/services/auth.service';
import { EditVacancyComponent } 
  from 'src/app/vacancies/components/edit-vacancy/edit-vacancy.component';
import { NotificationService } from '../../services/notification.service';
import { User } from 'src/app/users/models/user';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnDestroy {
  public value: string = '';
  public dropdownOpened: boolean = false;
  @Input() removeButton = false;

  user:User={
    firstName: "Emma",
  lastName: "Roberts",
  birthDate: new Date(1,1,2002),
  creationDate: new Date(1,1,2002),
  email: "emma.roberts@gmail.com",
  isEmailConfirmed: true,
  roles:[
    {
      name:'HR',
      key:1
    }
  ],
  image:"https://images.pexels.com/photos/733872/pexels-photo-733872.jpeg?cs=srgb&dl=pexels-andrea-piacquadio-733872.jpg&fm=jpg"
}

  public loading: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly service: AuthenticationService,
    private readonly dialog: MatDialog,
    private readonly notifications: NotificationService,
  ) {}

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }


  openDialog(): void {
    const dialogRef = this.dialog.open(EditVacancyComponent, {
      width: '914px',
      height: 'auto',
      data: {},
    });
  };
}
