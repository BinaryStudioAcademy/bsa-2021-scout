import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LoginRegistCommonComponent } from '../login-regist-common/login-regist-common.component';
import { RegisterDto } from '../../models/register-dto';
import { AuthenticationService } from '../../services/auth.service';
import { UserRegisterDto } from '../../models/auth/user-register-dto';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-registration-box',
  templateUrl: './registration-box.component.html',
  styleUrls: ['./registration-box.component.scss',
    '../login-regist-common/login-regist-common.component.scss'],
})
export class RegistrationBoxComponent {
  constructor(public loginRegistCommonComponent: LoginRegistCommonComponent,
    public authenticationService: AuthenticationService,
    private notificationService: NotificationService) { }

  public userRegisterDto: UserRegisterDto = {} as UserRegisterDto;

  public isPasswordHide = true;

  public registrationForm: FormGroup = new FormGroup({

    'userFirstName': new FormControl('', [
      Validators.required,
      this.loginRegistCommonComponent.firstAndLastNameValidation,
    ]),
    'userLastName': new FormControl('', [
      Validators.required,
      this.loginRegistCommonComponent.firstAndLastNameValidation,
    ]),
    'userEmail': new FormControl('', [
      Validators.required,
      Validators.pattern('^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_]+(\.[a-z0-9_-]+)*\.[a-z]{1,6}$'),
      this.loginRegistCommonComponent.noUnAllowedCharactersValidation,
    ], [this.loginRegistCommonComponent.userEmailValidator()]),
    'userPassword': new FormControl('', [
      Validators.required,
      this.loginRegistCommonComponent.minPasswordLenghtValidation,
      this.loginRegistCommonComponent.maxPasswordLenghtValidation,
      this.loginRegistCommonComponent.noWhitespaceValidation,
      this.loginRegistCommonComponent.upperCaseValidation,
      this.loginRegistCommonComponent.lowerCaseValidation,
      this.loginRegistCommonComponent.digitValidation,
      this.loginRegistCommonComponent.specialCharacterValidation,
      this.loginRegistCommonComponent.noUnAllowedCharactersValidation,
    ]),
    'userPasswordConfirmation': new FormControl('', [
      Validators.required,
    ]),
    'userBirthdate': new FormControl('', [
      Validators.required,
      this.loginRegistCommonComponent.birthDateValidation,
    ]),
  }, { validators: this.loginRegistCommonComponent.passwordsMatch });


  public onSubmit() {
    if (this.registrationForm.valid) {
      const dto: RegisterDto =
      {
        userRegisterDto: this.userRegisterDto,
        clientUrl: environment.confirmEmailUrl,
      };
      this.authenticationService.register(dto).pipe()
        .subscribe(() => {
          this.notificationService.showSuccessMessage(
            'Please check your email to confirm your email.');
        },
        () => this.notificationService.showErrorMessage('Something went wrong'));
    }
  }
}
