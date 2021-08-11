import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, 
  RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClientService } from 'src/app/shared/services/http-client.service';

@Injectable()
export class ResetPasswordGuard implements CanActivate {
  constructor(private router: Router, private httpClientService: HttpClientService) {}

  public canActivate(
    route: ActivatedRouteSnapshot, 
    state: RouterStateSnapshot)
    : Observable<boolean> {

    const email: string = route.queryParams['email'];
    const token: string = route.queryParams['token'];
    return this.httpClientService.getRequest<boolean>
    (`/Auth/reset-password?email=${email}&token=${token}`).pipe(
      map(isTokenValid => {
        if (isTokenValid) {
          return true;
        }
        this.router.navigate(['/']);
        return false;
      }));
  }
}
