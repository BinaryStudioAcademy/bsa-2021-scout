import { Injectable } from '@angular/core';
import { finalize, map, retry } from 'rxjs/operators';
import { HttpResponse } from '@angular/common/http';
import { AuthUser } from '../models/auth/auth-user';
import { UserLoginDto } from '../models/auth/user-login-dto';
import { Observable, of, throwError } from 'rxjs';
import { RefreshAccessTokenDto } from '../models/token/refresh-access-token-dto';
import { User } from 'src/app/users/models/user';
import { HttpClientService } from 'src/app/shared/services/http-client.service';

import { RegisterDto } from '../models/register-dto';
import { ConfirmEmailDto } from '../models/confirm-email-dto';
import { ResendConfirmEmailDto } from '../models/resend-confirm-email-dto';
import { ForgotPasswordDto } from '../models/forgot-password-dto';
import { ResetPasswordDto } from '../models/reset-password-dto';
import { AuthUserEventService } from './auth-user-event.service';
import { RegistrationLinkDto } from '../models/registration-link-dto';


@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private user: User | null = null;

  constructor(
    private httpService: HttpClientService,
    private authUserEventService: AuthUserEventService) { }

  public getUser(): Observable<User | null> {
    return this.user
      ? of(this.user)
      : this.httpService.getFullRequest<User>('/users/from-token').pipe(
        map((resp) => {
          if (!resp.body) {
            throw throwError(resp);
          }
          this.user = resp.body;
          this.authUserEventService.userChanged(this.user);
          return this.user;
        }),
      );
  }

  public setUser(user: User| null) {
    this.user = user;
    this.authUserEventService.userChanged(user);
  }

  public sendRegistrationLink(registrationLinkDto: RegistrationLinkDto) {
    return this.httpService.postFullRequest<void>(
      '/register/send-registration-link', registrationLinkDto);
  }

  public register(registerDto: RegisterDto): Observable<HttpResponse<void>> {
    return this.httpService.postFullRequest<void>('/register', registerDto);
  }

  public resendConfirmationEmail(resendConfirmEmailDto: ResendConfirmEmailDto):
  Observable<HttpResponse<void>> {
    return this.httpService.postFullRequest<void>('/register/resend-confirm-email',
      resendConfirmEmailDto);
  }

  public confirmEmail(confirmEmailDto: ConfirmEmailDto): Observable<User> {
    return this._handleAuthResponse(
      this.httpService.postFullRequest<AuthUser>('/register/confirm-email', confirmEmailDto));
  }

  public login(user: UserLoginDto): Observable<User> {
    return this._handleAuthResponse(
      this.httpService.postFullRequest<AuthUser>('/auth/login', user));
  }

  public logout(): Observable<HttpResponse<void>> {
    return this.revokeRefreshToken().pipe(
      finalize(() => {
        this.removeTokensFromStorage();
        this.user = null;
        this.authUserEventService.userChanged(null);
      }),
    );
  }

  public areTokensExist(): boolean {
    return localStorage.getItem('accessToken') !== null &&
      localStorage.getItem('refreshToken') !== null;
  }

  private revokeRefreshToken(): Observable<HttpResponse<void>> {
    return this.httpService.postFullRequest('/token/revoke', {
      refreshToken: JSON.parse(localStorage.getItem('refreshToken') as string),
    });
  }

  public removeTokensFromStorage(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
  }

  public refreshTokens(): Observable<RefreshAccessTokenDto> {
    return this.httpService
      .postFullRequest<RefreshAccessTokenDto>('/token/refresh', {
      accessToken: JSON.parse(localStorage.getItem('accessToken') as string),
      refreshToken: JSON.parse(localStorage.getItem('refreshToken') as string),
    })
      .pipe(
        map((resp) => {
          if (!resp.body) {
            throw throwError(resp);
          }
          this._setTokens(resp.body);
          return resp.body;
        }),
      );
  }

  public isEmailExist(email: string): Observable<boolean> {
    return this.httpService.getRequest<boolean>
    (`/Users/Email/${email}`);
  }

  public sendPasswordResetRequest(forgotPasswordDto: ForgotPasswordDto): Observable<void> {
    return this.httpService.postRequest<void>('/Auth/forgot-password', forgotPasswordDto);
  }

  public isResetTokenPasswordValid(email: string, token: string): Observable<boolean> {
    return this.httpService.getRequest<boolean>
    (`/Auth/reset-password?email=${email}&token=${token}`);
  }

  public resetPassword(resetPasswordDto: ResetPasswordDto): Observable<void> {
    return this.httpService.postRequest<void>('/Auth/reset-password', resetPasswordDto);
  }

  private _handleAuthResponse(observable: Observable<HttpResponse<AuthUser>>) {
    return observable.pipe(
      map((resp) => {
        if (!resp.body) {
          throw throwError(resp);
        }
        this._setTokens(resp.body.token);
        this.user = resp.body.user;
        this.authUserEventService.userChanged(resp.body.user);
        return resp.body.user;
      }),
    );
  }

  private _setTokens(tokens: RefreshAccessTokenDto) {
    if (tokens && tokens.accessToken && tokens.refreshToken) {
      localStorage.setItem('accessToken', JSON.stringify(tokens.accessToken.token));
      localStorage.setItem('refreshToken', JSON.stringify(tokens.refreshToken));
    }
  }
}
