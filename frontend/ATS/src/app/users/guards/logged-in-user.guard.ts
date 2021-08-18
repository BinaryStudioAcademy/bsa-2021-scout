import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AppRoute } from 'src/app/routing/AppRoute';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { AuthenticationService } from '../services/auth.service';


@Injectable()
export class LoggedInUserGuard implements CanActivate {
  constructor(
    private router: Router,
    private authService: AuthenticationService,
    private notificationsService: NotificationService,
  ) { }

  public canActivate(): boolean {
    if (this.authService.areTokensExist()) {
      this.router.navigate([`/${AppRoute.Home}`]);
      this.notificationsService.showInfoMessage('You are already logged in',
        'Reminding');
      return false;
    }

    return true;
  }
}