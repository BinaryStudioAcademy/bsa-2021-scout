import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  public isPasswordHide = true;

  public loginForm: FormGroup = new FormGroup({
    'userEmail': new FormControl('', [
      Validators.required,
      Validators.pattern('^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_]+(\.[a-z0-9_-]+)*\.[a-z]{1,6}$'),
      this.noUnAllowedCharactersValidation,
    ]),
    'userPassword': new FormControl('', [
      Validators.required,
      this.minPasswordLenghtValidation,
      this.maxPasswordLenghtValidation,
      this.noWhitespaceValidation,
      this.upperCaseValidation,
      this.lowerCaseValidation,
      this.digitValidation,
      this.specialCharacterValidation,
      this.noUnAllowedCharactersValidation,
    ]),
  });

  public checkIsControlValid(cotrolName: string) {
    return this.loginForm.controls[cotrolName].invalid && this.loginForm.controls[cotrolName].dirty;
  }

  public noWhitespaceValidation(control: FormControl) {
    return (control.value as string || '').indexOf(' ') == -1 ? null : { 'whitespace': true };
  }

  public upperCaseValidation(control: FormControl) {
    return (control.value as string || '').match(/[A-Z]/) != null ? null : { 'uppercase': true };
  }

  public lowerCaseValidation(control: FormControl) {
    return (control.value as string || '').match(/[a-z]/) != null ? null : { 'lowercase': true };
  }

  public digitValidation(control: FormControl) {
    return (control.value as string || '').match(/[0-9]/) != null ? null : { 'digit': true };
  }

  public specialCharacterValidation(control: FormControl) {
    return (control.value as string || '')
      .match(/[$-/:-?{-~!"^_`\[\]]/) != null ? null : { 'specialcharacters': true };
  }

  public noUnAllowedCharactersValidation(control: FormControl) {
    return (control.value as string || '')
      .match(/[^a-zA-Z0-9$ -/@:-?{-~!"^_`\[\]]/) == null ? null : { 'unallowedcharacters': true };
  }

  public minPasswordLenghtValidation(control: FormControl) {
    return (control.value as string || '').length >= 8 ? null : { 'minpasswordlenght': true };
  }

  public maxPasswordLenghtValidation(control: FormControl) {
    return (control.value as string || '').length <= 32 ? null : { 'maxpasswordlenght': true };
  }
}
