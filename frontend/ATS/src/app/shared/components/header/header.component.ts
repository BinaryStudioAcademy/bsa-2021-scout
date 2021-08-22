import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import _ from 'lodash';
import { AuthenticationService } from 'src/app/users/services/auth.service';
import { EditVacancyComponent } 
  from 'src/app/vacancies/components/edit-vacancy/edit-vacancy.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
  public value: string = '';
  public dropdownOpened: boolean = false;
  @Input() removeButton = false;

  public constructor(
    private readonly service: AuthenticationService,
    private readonly dialog: MatDialog,
  ) {}

  public logout(): void {
    this.service.logout().subscribe(
      () => window.location.replace('/login'),
    );
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(EditVacancyComponent, {
      width: '914px',
      height: 'auto',
      data: {},
    });
  };
}
