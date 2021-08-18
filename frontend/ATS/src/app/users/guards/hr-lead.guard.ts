import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, 
  RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthenticationService } from '../services/auth.service';

@Injectable()
export class HrLeadGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthenticationService) {}

  public canActivate(
    route: ActivatedRouteSnapshot, 
    state: RouterStateSnapshot)
    : Observable<boolean> {
    return this.authService.getUser().pipe(
      map(user => {
        if (user?.roles?.find(role => role.name === 'HrLead')) {
          return true;
        }
        this.router.navigate(['/']);
        return false;
      }));
  }
}
