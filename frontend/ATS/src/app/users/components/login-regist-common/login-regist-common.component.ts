import { Component } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AsyncValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { connectableObservableDescriptor } from 'rxjs/internal/observable/ConnectableObservable';
import { catchError, map } from 'rxjs/operators';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-login-regist-common',
  templateUrl: './login-regist-common.component.html',
  styleUrls: ['./login-regist-common.component.scss'],
})
export class LoginRegistCommonComponent {

  constructor(private httpClientService: HttpClientService,
    private notificationService: NotificationService) { }

  public checkIsControlValid(form: FormGroup, cotrolName: string) {
    return form.controls[cotrolName].invalid && form.controls[cotrolName].dirty;
  }

  public checkIsPasswordValid(form: FormGroup, cotrolName: string) {
    return (form.controls[cotrolName].value as string).length > 0
      && (form.controls[cotrolName].errors?.whitespace
        || form.controls[cotrolName].errors?.uppercase
        || form.controls[cotrolName].errors?.lowercase
        || form.controls[cotrolName].errors?.digit
        || form.controls[cotrolName].errors?.specialcharacters
        || form.controls[cotrolName].errors?.unallowedcharacters
        || form.controls[cotrolName].errors?.minpasswordlenght
        || form.controls[cotrolName].errors?.maxpasswordlenght);
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
      .match(/^[A-Z]+(([',. -][a-zA-Z])?[a-zA-Z]*)*$/) != null ?
      null : { 'firstandlastname': true };
  }

  public minPasswordLenghtValidation(control: FormControl) {
    return (control.value as string || '').length >= 8 ? null : { 'minpasswordlenght': true };
  }

  public maxPasswordLenghtValidation(control: FormControl) {
    return (control.value as string || '').length <= 32 ? null : { 'maxpasswordlenght': true };
  }

  public passwordsMatch(c: AbstractControl): { invalid: boolean, mismatch: boolean } | null {
    if (c.get('userPassword')?.dirty
      && c.get('userPasswordConfirmation')?.dirty
      && (c.get('userPassword')?.value
        != c.get('userPasswordConfirmation')?.value)) {
      return { invalid: true, mismatch: true };
    }
    return null;
  }

  public birthDateValidation(control: FormControl) {
    if (control.value != null) {
      let enterdDate: Date = new Date(control.value);
      return enterdDate < new Date()
        && enterdDate > new Date('01-01-1900')
        ? null : { 'datevalidate': true };
    }
    return null;
  }

  userEmailValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      return this.httpClientService.getRequest(`/Users/Email/${control.value}`).pipe(
        map(res => {
          if (res) {
            this.notificationService.showErrorMessage('Someone\'s already using that email.');
            return { userEmailExists: true };
          }
          return null;
        }), catchError(err => {
          this.notificationService.showErrorMessage('Email uniqueness check error.');
          return of([{ userEmailExists: true }]);
        }),
      );
    };
  }
}