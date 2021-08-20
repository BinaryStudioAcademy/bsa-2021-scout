import { Component, Input, OnDestroy } from '@angular/core';
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
  @Input() removeButton = false;

  public loading: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly service: AuthenticationService,
    private readonly notifications: NotificationService,
  ) {}

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
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
          this.notifications.showErrorMessage('Failed to log out.');
        },
      );
  }
}
