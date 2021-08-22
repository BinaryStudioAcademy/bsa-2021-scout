import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { EMPTY, Subject } from 'rxjs';
import { takeUntil, mergeMap } from 'rxjs/operators';
import { AppRoute } from 'src/app/routing/AppRoute';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { environment } from 'src/environments/environment';
import { ForgotPasswordDto } from '../../models/forgot-password-dto';
import { AuthenticationService } from '../../services/auth.service';
import { LoginRegistCommonComponent } from '../login-regist-common/login-regist-common.component';

@Component({
  selector: 'app-forgot-password-dialog',
  templateUrl: './forgot-password-dialog.component.html',
  styleUrls: ['./forgot-password-dialog.component.scss'],
})
export class ForgotPasswordDialogComponent implements OnDestroy {
  constructor(
    private dialogRef: MatDialogRef<ForgotPasswordDialogComponent>,
    public loginRegistCommonComponent: LoginRegistCommonComponent,
    private notificationService: NotificationService,
    private authService: AuthenticationService,
  ) {}

  public isRequestFinished = true;

  public emailForm: FormGroup = new FormGroup({
    userEmail: new FormControl('', [
      Validators.required,
      Validators.pattern(
        '^([a-zA-Z0-9_-]+.)*[a-zA-Z0-9_-]+@[a-zA-Z0-9_]+(.[a-zA-Z0-9_-]+)*.[a-zA-Z]{1,6}$',
      ),
      this.loginRegistCommonComponent.noUnAllowedCharactersValidation,
    ]),
  });

  public loading: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public resetPassword(): void {
    if (this.emailForm.valid) {
      this.loading = true;
      this.authService
        .isEmailExist(this.emailForm.get('userEmail')?.value)
        .pipe(
          takeUntil(this.unsubscribe$),
          mergeMap(isEmailExist => {
            if (isEmailExist) {
              const forgotPasswordDto: ForgotPasswordDto = 
              { 
                email: this.emailForm.get('userEmail')?.value, 
                clientURI: `${environment.clientUrl}/${AppRoute.ResetPassword}`, 
              };
              return this.authService.sendPasswordResetRequest(forgotPasswordDto);      
            }
            this.notificationService.showErrorMessage('There is no user with such email address.');
            return EMPTY;
          }),
        )
        .subscribe(
          () => {
            this.loading = false;
            this.notificationService.showSuccessMessage(
              'Please check your email to reset your password');
            this.dialogRef.close();
          },
          () => {
            this.loading = false;
            this.notificationService.showErrorMessage('Something went wrong');
          },
        );
    }  
  }
}
