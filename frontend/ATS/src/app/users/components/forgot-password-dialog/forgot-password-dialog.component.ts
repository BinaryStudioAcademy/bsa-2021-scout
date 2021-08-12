import {Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { EMPTY } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
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
export class ForgotPasswordDialogComponent {

  constructor(
    private dialogRef: MatDialogRef<ForgotPasswordDialogComponent>,
    public loginRegistCommonComponent: LoginRegistCommonComponent,
    private notificationService: NotificationService,
    private authService: AuthenticationService) {}

  public emailForm: FormGroup = new FormGroup({
    'userEmail': new FormControl('', [
      Validators.required,
      Validators.pattern('^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_]+(\.[a-z0-9_-]+)*\.[a-z]{1,6}$'),
      this.loginRegistCommonComponent.noUnAllowedCharactersValidation,
    ]),
  });

  public resetPassword(): void {
    this.authService.isEmailExist(this.emailForm.get('userEmail')?.value)
      .pipe(
        mergeMap(isEmailExist => {
          if (isEmailExist) {
            const forgotPasswordDto: ForgotPasswordDto = 
            { 
              email: this.emailForm.get('userEmail')?.value, 
              clientURI: `${environment.clientUrl}/reset-password`, 
            };
            return this.authService.sendPasswordResetRequest(forgotPasswordDto);      
          }
          this.notificationService.showErrorMessage('There is no user with such email address.');
          return EMPTY;
        }),
      )
      .subscribe(() => {
        this.notificationService.showSuccessMessage(
          'Please check your email to reset your password');
        this.dialogRef.close();
      },
      () => this.notificationService.showErrorMessage('Something went wrong'));
  }
}