import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import {
  animateText,
  onSideNavChange,
} from 'src/app/shared/animations/animation';
import { SidenavService } from 'src/app/shared/services/sidenav.service';
import { User } from 'src/app/users/models/user';
import { AuthUserEventService } from 'src/app/users/services/auth-user-event.service';
import { AuthenticationService } from 'src/app/users/services/auth.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
  animations: [onSideNavChange, animateText],
})
export class MenuComponent implements OnInit, OnDestroy {
  public hideMenu = false;
  public sideNavState: boolean = true;
  public linkText: boolean = true;
  public isHrLead: boolean = false;
  public loading: boolean = true;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private _sidenavService: SidenavService,
    private authService: AuthenticationService,
    private authUserEventService: AuthUserEventService,
    private notifications: NotificationService,
  ) {}

  public ngOnInit() {
    this.authService
      .getUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (user) => {
          this.loading = false;
          this.checkIsHrLead(user);
        },
        () => {
          this.loading = false;
          this.notifications.showErrorMessage('Failed to check user role.');
        },
      );

    this.authUserEventService.userChangedEvent$
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((user) => this.checkIsHrLead(user));
  }

  public ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  onSinenavToggle() {
    this.sideNavState = !this.sideNavState;

    setTimeout(() => {
      this.linkText = this.sideNavState;
    }, 200);
    this._sidenavService.sideNavState$.next(this.sideNavState);
  }

  public checkIsHrLead(user: User | null): void {
    this.isHrLead = user?.roles?.find((role) => role.name === 'HrLead')
      ? true
      : false;
  }
}
