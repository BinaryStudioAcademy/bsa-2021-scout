import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { EmailIsNotConfirmedErrorType } 
  from '../../models/auth/emai-is-not-confirmed-error-type';
import { UserLoginDto } from '../../models/auth/user-login-dto';
import { AuthenticationService } from '../../services/auth.service';
import { ForgotPasswordDialogComponent }
  from '../forgot-password-dialog/forgot-password-dialog.component';
import { LoginRegistCommonComponent }
  from '../login-regist-common/login-regist-common.component';

@Component({
  selector: 'app-login-box',
  templateUrl: './login-box.component.html',
  styleUrls: ['./login-box.component.scss',
    '../login-regist-common/login-regist-common.component.scss'],
})
export class LoginBoxComponent {

  public constructor(
    public loginRegistCommonComponent: LoginRegistCommonComponent,
    public dialog: MatDialog,
    public authenticationService: AuthenticationService,
    private notificationService: NotificationService,
    private router: Router) { }

  public userLoginDto: UserLoginDto = {} as UserLoginDto;

  public isPasswordHide = true;

  public isRequestFinished = true;

  public loginForm: FormGroup = new FormGroup({
    'userEmail': new FormControl('', [
      Validators.required,
      Validators
        .pattern(
          '^([a-zA-Z0-9_-]+\.)*[a-zA-Z0-9_-]+@[a-zA-Z0-9_]+(\.[a-zA-Z0-9_-]+)*\.[a-zA-Z]{1,6}$',
        ),
      this.loginRegistCommonComponent.noUnAllowedCharactersValidation,
      this.loginRegistCommonComponent.onlyOneAtSign,
    ]),
    'userPassword': new FormControl('', [
      Validators.required,
    ]),
  });

  public openDialog(): void {
    this.dialog.open(ForgotPasswordDialogComponent, {
      disableClose: true,
      maxWidth: '400px',
    });
  }

  public onSubmit() {
    if (this.loginForm.valid) {

      this.authenticationService.login(this.userLoginDto).pipe()
        .subscribe(() => {
          this.isRequestFinished = false;
          this.router.navigate(['/']);
        },
        (error) => {
          this.isRequestFinished = true;
          if (error.description != null) {
            if (error.type === EmailIsNotConfirmedErrorType.EmailIsNotConfirmed) {
              this.router.navigate(['/resend-email'],
                { queryParams: { email: this.userLoginDto.email } });
            }
            else {
              this.notificationService.showErrorMessage(error.description, 'Something went wrong');
            }
          }
          else {
            this.notificationService.showErrorMessage(error, 'Something went wrong');
          }
        });
    }
  }
}
