import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/auth.service';
import { TokenErrorType } from '../models/auth/token-error-type';
import { EmailIsNotConfirmedErrorType } from '../models/auth/emai-is-not-confirmed-error-type';
import { EmailIsAlreadyConfirmedErrorType } from
  '../models/auth/emai-is-already-confirmed-error-type';
import { NotificationService } from 'src/app/shared/services/notification.service';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private authService: AuthenticationService,
    private notificationService: NotificationService) { }

  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((response) => {
        if (response.status === 404) {

          if (response.error.message != null) {
            const errorInfo: { description: string; }
              = { description: response.error.message };
            return throwError(errorInfo);
          }

          const errorInfo: { type: string; description: string }
            = JSON.parse(response.error.message);

          if (errorInfo.type === EmailIsAlreadyConfirmedErrorType.EmailIsAlreadyConfirmed) {
            return throwError(errorInfo);
          }
        }

        if (response.status === 401) {

          if (response.error?.message) {
            const errorInfo: { type: string; description: string }
              = JSON.parse(response.error.message);
            if (errorInfo.type === TokenErrorType.InvalidToken) {
              if (this.authService.areTokensExist()) {
                this.authService.removeTokensFromStorage();
              }
              this.router.navigate(['/login']);
              this.authService.setUser(null);     
              this.notificationService.showWarningMessage('You need to re-login. Sorry.');
              return throwError(errorInfo);
            }
            if (errorInfo.type === TokenErrorType.ExpiredRefreshToken) {
              this.router.navigate(['/login']);
              this.authService.logout().subscribe(
                () => console.log('expired refresh token is deleted'));
              this.notificationService.showWarningMessage('You need to re-login. Sorry.');
              return throwError(errorInfo);
            }
            if (errorInfo.type === EmailIsNotConfirmedErrorType.EmailIsNotConfirmed) {
              return throwError(errorInfo);
            }
          }

          if (response.headers.has('Token-Expired')) {
            return this.authService.refreshTokens().pipe(
              switchMap((resp) => {
                if (req.body?.refreshToken) {
                  req = req.clone({
                    setHeaders: {
                      Authorization: `Bearer ${resp.accessToken.token}`,
                    },
                    body: { refreshToken: resp.refreshToken },
                  });
                } else {
                  req = req.clone({
                    setHeaders: {
                      Authorization: `Bearer ${resp.accessToken.token}`,
                    },
                  });
                }
                return next.handle(req);
              }),
            );
          }
        }

        if (response.status === 400) {
          const errorInfo: { type: string; description: string }
            = JSON.parse(response.error.message);
          if (errorInfo.type === EmailIsAlreadyConfirmedErrorType.EmailIsAlreadyConfirmed) {
            return throwError(errorInfo);
          }
        }

        const error = response.error && response.error.message
          ? response.error.message
          : response.message || `${response.status} ${response.statusText}`;
        return throwError(error);
      }),
    );
  }
}