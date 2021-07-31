import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LoginRegistCommonComponent } from '../login-regist-common/login-regist-common.component';

@Component({
  selector: 'app-login-box',
  templateUrl: './login-box.component.html',
  styleUrls: ['./login-box.component.scss',
    '../login-regist-common/login-regist-common.component.scss'],
})
export class LoginBoxComponent {

  public constructor(public loginRegistCommonComponent: LoginRegistCommonComponent) { }

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

}
