import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { EditHrFormComponent } from 'src/app/users/components/edit-hr-form/edit-hr-form.component';
import { EditHrsDialogComponent } from 'src/app/users/components/edit-hrs-dialog/edit-hrs-dialog.component';
import { User } from '../../../users/models/user';
import { AuthenticationService } from '../../../users/services/auth.service';

@Component({
  selector: 'app-profile-dropdown-menu',
  templateUrl: './profile-dropdown-menu.component.html',
  styleUrls: ['./profile-dropdown-menu.component.scss']
})
export class ProfileDropdownMenuComponent implements OnInit {
  
  public loading: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly service: AuthenticationService,
    private dialog: MatDialog
  ) {}
  user:User = {
    firstName: "Emma",
  lastName: "Roberts",
  birthDate: new Date(1,1,2002),
  creationDate: new Date(1,1,2002),
  email: "aaa",
  isEmailConfirmed: true,
  roles:[
    {
      name:'HR',
      key:1
    }
  ]
  }

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
  ngOnInit(): void {
  }

  // openDialog(): void {
  //   const dialogRef = this.dialog.open(EditHrsDialogComponent, {
  //     width: '90%',
  //     height: 'auto',
  //     data: {},
  //   });

  //   // dialogRef.afterClosed().subscribe(() => this.getVacancies());
  // }

  public editHr(){
    const dialogRef = this.dialog.open(EditHrsDialogComponent, {
      width: '90%',
      height: 'auto',
      data: {},
    });
  }

  onOpenProfile(){
    const dialogRef = this.dialog.open(EditHrFormComponent, {
      width: '90%',
      height: 'auto',
      data: {},
    });
  }

}
