import { Component, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { mergeMap, takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { EmailIsNotConfirmedErrorType } from '../../models/auth/emai-is-not-confirmed-error-type';
import { UserLoginDto } from '../../models/auth/user-login-dto';
import { AuthenticationService } from '../../services/auth.service';

import {
  ForgotPasswordDialogComponent,
} from '../forgot-password-dialog/forgot-password-dialog.component';

import { LoginRegistCommonComponent } from '../login-regist-common/login-regist-common.component';

@Component({
  selector: 'app-login-box',
  templateUrl: './login-box.component.html',
  styleUrls: [
    './login-box.component.scss',
    '../login-regist-common/login-regist-common.component.scss',
  ],
})
export class LoginBoxComponent implements OnDestroy {
  public constructor(
    public loginRegistCommonComponent: LoginRegistCommonComponent,
    public dialog: MatDialog,
    public authenticationService: AuthenticationService,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  public userLoginDto: UserLoginDto = {} as UserLoginDto;
  public isPasswordHide = true;
  public loading: boolean = false;

  public loginForm: FormGroup = new FormGroup({
    userEmail: new FormControl('', [
      Validators.required,
      Validators.pattern(
        '^([a-zA-Z0-9_-]+.)*[a-zA-Z0-9_-]+@[a-zA-Z0-9_]+(.[a-zA-Z0-9_-]+)*.[a-zA-Z]{1,6}$',
      ),
      this.loginRegistCommonComponent.noUnAllowedCharactersValidation,
      this.loginRegistCommonComponent.onlyOneAtSign,
    ]),
    userPassword: new FormControl('', [Validators.required]),
  });

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public openDialog(): void {
    this.dialog.open(ForgotPasswordDialogComponent, {
      disableClose: true,
      maxWidth: '400px',
    });
  }

  public onSubmit() {
    if (this.loginForm.valid) {
      this.loading = true;

      this.authenticationService
        .login(this.userLoginDto)
        .pipe(
          takeUntil(this.unsubscribe$),
          mergeMap(() => this.route.queryParams))
        .subscribe(
          (queryParams) => {
            this.loading = false;
            if (queryParams.link) {
              this.router.navigateByUrl(atob(queryParams.link));
            }
            else {
              this.router.navigate(['/']);
            }       
          },
          (error) => {
            this.loading = false;

            if (error.description != null) {
              if (
                error.type === EmailIsNotConfirmedErrorType.EmailIsNotConfirmed
              ) {
                this.router.navigate(['/resend-email'], {
                  queryParams: { email: this.userLoginDto.email },
                });
              } else {
                this.notificationService.showErrorMessage(
                  error.description,
                  'Something went wrong',
                );
              }
            } else {
              this.notificationService.showErrorMessage(
                error,
                'Something went wrong',
              );
            }
          },
        );
    }
  }
}
