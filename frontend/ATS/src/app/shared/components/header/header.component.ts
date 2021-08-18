import { Component, OnDestroy } from '@angular/core';
import _ from 'lodash';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/users/services/auth.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnDestroy {
  public value: string = '';
  public dropdownOpened: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly service: AuthenticationService,
    private readonly notifications: NotificationService,
  ) {}

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public toggleDropdown(): void {
    this.dropdownOpened = !this.dropdownOpened;
  }

  public logout(): void {
    this.service
      .logout()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        () => window.location.replace('/login'),
        () => this.notifications.showErrorMessage('Failed to log out.'),
      );
  }
}
