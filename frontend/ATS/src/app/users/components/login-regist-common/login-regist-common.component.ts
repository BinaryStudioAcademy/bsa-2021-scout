import { Component } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AsyncValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-login-regist-common',
  templateUrl: './login-regist-common.component.html',
  styleUrls: ['./login-regist-common.component.scss'],
})
export class LoginRegistCommonComponent {

  public checkIsControlValid(form : FormGroup , cotrolName: string) {
    return form.controls[cotrolName].invalid && form.controls[cotrolName].dirty;
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

  public firstAndLastNameValidation(control: FormControl) {
    return (control.value as string || '')
      .match(/^[A-Z]+(([',. -][a-zA-Z])?[a-zA-Z]*)*$/) != null 
      ? null : { 'firstandlastname': true };
  }

  public minPasswordLenghtValidation(control: FormControl) {
    return (control.value as string || '').length >= 8 ? null : { 'minpasswordlenght': true };
  }

  public maxPasswordLenghtValidation(control: FormControl) {
    return (control.value as string || '').length <= 32 ? null : { 'maxpasswordlenght': true };
  }
  
  public passwordsMatch(form : FormGroup, firstPassword : string, secondPassword : string){
    return form.controls[firstPassword].dirty
    && form.controls[secondPassword].dirty 
    && (form.controls[firstPassword].value 
    != form.controls[secondPassword].value);
  }

  public birthDateValidation(control: FormControl){
    let enterdDate : Date = new Date(control.value);
    return enterdDate < new Date() 
    && enterdDate > new Date('01-01-1900') 
      ? null : { 'datevalidate': true };
  }
}