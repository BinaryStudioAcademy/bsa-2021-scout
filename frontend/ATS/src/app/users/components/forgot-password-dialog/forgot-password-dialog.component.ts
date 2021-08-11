import {Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { EMPTY } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ForgotPasswordDto } from '../../models/forgot-password-dto';
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
    private httpClientService: HttpClientService) {}


  public emailForm: FormGroup = new FormGroup({
    'userEmail': new FormControl('', [
      Validators.required,
      Validators.pattern('^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_]+(\.[a-z0-9_-]+)*\.[a-z]{1,6}$'),
      this.loginRegistCommonComponent.noUnAllowedCharactersValidation,
    ]),
  });

  public resetPassword(): void {
    this.httpClientService.getRequest<boolean>
    (`/Users/Email/${this.emailForm.get('userEmail')?.value}`).pipe(
      mergeMap(isEmailExist => {
        if (isEmailExist) {
          const dto: ForgotPasswordDto = 
          { 
            email: this.emailForm.get('userEmail')?.value, 
            clientURI: 'http://localhost:4200/reset-password', 
          };
          return this.httpClientService.postFullRequest<void>('/Auth/forgot-password', dto);      
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
      () => this.notificationService.showErrorMessage('Something went wrong') );
  }
}