import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { UserLoginDto } from '../../models/auth/user-login-dto';
import { AuthenticationService } from '../../services/auth.service';
import { ForgotPasswordDialogComponent } 
  from '../forgot-password-dialog/forgot-password-dialog.component';
import { LoginRegistCommonComponent } from '../login-regist-common/login-regist-common.component';

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

  public userLoginDto : UserLoginDto = {} as UserLoginDto;

  public isPasswordHide = true;

  public loginForm: FormGroup = new FormGroup({
    'userEmail': new FormControl('', [
      Validators.required,
      Validators.pattern('^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_]+(\.[a-z0-9_-]+)*\.[a-z]{1,6}$'),
      this.loginRegistCommonComponent.noUnAllowedCharactersValidation,
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

  public onSubmit(){
    if(this.loginForm.valid){
      this.authenticationService.login(this.userLoginDto).pipe()
        .subscribe(() => {
          this.router.navigate(['/']);
        },
        () => this.notificationService.showErrorMessage('Something went wrong'));
    }
  }
}
