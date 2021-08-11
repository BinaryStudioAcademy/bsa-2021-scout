import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LoginRegistCommonComponent } from '../login-regist-common/login-regist-common.component';
import { RegisterDto } from '../../models/register-dto';
import { AuthenticationService } from '../../services/auth.service';
import { UserRegisterDto } from '../../models/auth/user-register-dto';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration-box',
  templateUrl: './registration-box.component.html',
  styleUrls: ['./registration-box.component.scss',
    '../login-regist-common/login-regist-common.component.scss'],
})
export class RegistrationBoxComponent {
  constructor(public loginRegistCommonComponent: LoginRegistCommonComponent,
    public authenticationService: AuthenticationService,
    private notificationService: NotificationService,
    private router: Router) { }

  public userRegisterDto: UserRegisterDto = {} as UserRegisterDto;

  public isPasswordHide = true;

  public isRequestFinished = true;

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
      Validators
        .pattern(
          '^([a-zA-Z0-9_-]+\.)*[a-zA-Z0-9_-]+@[a-zA-Z0-9_]+(\.[a-zA-Z0-9_-]+)*\.[a-zA-Z]{1,6}$',
        ),
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
      this.isRequestFinished = false;
      console.log(this.isRequestFinished);
      this.authenticationService.register(dto).pipe()
        .subscribe(() => {
          this.isRequestFinished = true;
          this.router.navigate(['/successful-registration'],
            { queryParams: { email: this.userRegisterDto.email } });
        },
        (error) => {
          this.isRequestFinished = true;
          this.notificationService.showErrorMessage(error.description, 'Something went wrong');
        },
        );

    }
  }
}
