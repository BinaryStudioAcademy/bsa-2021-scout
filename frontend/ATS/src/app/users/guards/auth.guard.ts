import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CanActivate, CanActivateChild, Router, ActivatedRouteSnapshot, 
  RouterStateSnapshot } from '@angular/router';
import { AppRoute } from 'src/app/routing/AppRoute';
import { AuthenticationService } from '../services/auth.service';


@Injectable()
export class AuthGuard implements CanActivateChild, CanActivate {
  constructor(
    private router: Router, 
    private authService: AuthenticationService,
    private dialog: MatDialog,
  ) {}

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.checkForActivation(state);
  }

  public canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.checkForActivation(state);
  }

  private checkForActivation(state: RouterStateSnapshot) {
    if (this.authService.areTokensExist()) {
      return true;
    }

    this.router.navigate([AppRoute.Login], { 
      queryParams: { link: btoa(state.url) },
      queryParamsHandling: 'merge',
    });

    setTimeout(this.dialog.closeAll, 200);

    return false;
  }
}
