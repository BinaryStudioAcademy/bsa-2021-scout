import { Component } from '@angular/core';
import _ from 'lodash';
import { AuthenticationService } from 'src/app/users/services/auth.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
  public value: string = '';
  public dropdownOpened: boolean = false;

  public constructor(
    private readonly service: AuthenticationService,
    private readonly notifications: NotificationService,
  ) {}

  public toggleDropdown(): void {
    this.dropdownOpened = !this.dropdownOpened;
  }

  public logout(): void {
    this.service.logout().subscribe(
      () => window.location.replace('/login'),
      () => this.notifications.showErrorMessage('Failed to log out.'),
    );
  }
}
