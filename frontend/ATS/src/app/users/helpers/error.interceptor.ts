import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/auth.service';
import { TokenErrorType } from '../models/auth/token-error-type';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private authService: AuthenticationService) {}

  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((response) => {
        if (response.status === 401) {
          
          if (response.headers.has('Token-Expired')) {
            return this.authService.refreshTokens().pipe(
              switchMap((resp) => {
                if (req.body.refreshToken) {
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
          
          const errorInfo: { type: string; description: string} 
          = JSON.parse(response.error.message);
          if (errorInfo) {
            if (errorInfo.type === TokenErrorType.InvalidToken && 
                !this.authService.areTokensExist()) {
              return throwError(errorInfo);
            }
            if (errorInfo.type === TokenErrorType.ExpiredRefreshToken) {
              this.router.navigate(['/']);
              this.authService.logout().subscribe(
                () => console.log('expired refresh token is deleted'), 
                (error) => console.log(error));
              return throwError(errorInfo);
            }
          }         
        }

        const error = response.error.message
          ? response.error.message
          : response.message || `${response.status} ${response.statusText}`;

        return throwError(error);
      }),
    );
  }
}
