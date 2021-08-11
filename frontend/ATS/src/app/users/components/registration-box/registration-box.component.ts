import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LoginRegistCommonComponent } from '../login-regist-common/login-regist-common.component';

@Component({
  selector: 'app-registration-box',
  templateUrl: './registration-box.component.html',
  styleUrls: ['./registration-box.component.scss', 
    '../login-regist-common/login-regist-common.component.scss'],
})
export class RegistrationBoxComponent {
  constructor(public loginRegistCommonComponent: LoginRegistCommonComponent) {
  }

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

  onSubmit(){
  }
}
