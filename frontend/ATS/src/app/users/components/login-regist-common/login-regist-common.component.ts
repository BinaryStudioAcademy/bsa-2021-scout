import { Component, OnDestroy } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import {
  AsyncValidatorFn,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { Observable, of, Subject } from 'rxjs';
import { catchError, map, takeUntil } from 'rxjs/operators';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import moment from 'moment';

@Component({
  selector: 'app-login-regist-common',
  templateUrl: './login-regist-common.component.html',
  styleUrls: ['./login-regist-common.component.scss'],
})
export class LoginRegistCommonComponent implements OnDestroy {
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private httpClientService: HttpClientService,
    private notificationService: NotificationService,
  ) {}

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public checkIsControlValid(form: FormGroup, cotrolName: string) {
    return form.controls[cotrolName].invalid && form.controls[cotrolName].dirty;
  }

  public checkIsPasswordValid(form: FormGroup, cotrolName: string) {
    return (
      (form.controls[cotrolName].value as string)?.length > 0 &&
      (form.controls[cotrolName].errors?.uppercase ||
        form.controls[cotrolName].errors?.lowercase ||
        form.controls[cotrolName].errors?.digit ||
        form.controls[cotrolName].errors?.specialcharacters ||
        form.controls[cotrolName].errors?.unallowedcharacters ||
        form.controls[cotrolName].errors?.minpasswordlenght ||
        form.controls[cotrolName].errors?.maxpasswordlenght)
    );
  }

  public onlyOneAtSign(control: FormControl) {
    return (((control.value as string) || '').match(/@/g)?.length as number) <=
      1 || ((control.value as string) || '').match(/@/g)?.length == undefined
      ? null
      : { onlyoneatsign: true };
  }

  public noWhitespaceValidation(control: FormControl) {
    return ((control.value as string) || '').indexOf(' ') == -1
      ? null
      : { whitespace: true };
  }

  public upperCaseValidation(control: FormControl) {
    return ((control.value as string) || '').match(/[A-Z]/) != null
      ? null
      : { uppercase: true };
  }

  public lowerCaseValidation(control: FormControl) {
    return ((control.value as string) || '').match(/[a-z]/) != null
      ? null
      : { lowercase: true };
  }

  public digitValidation(control: FormControl) {
    return ((control.value as string) || '').match(/[0-9]/) != null
      ? null
      : { digit: true };
  }

  public specialCharacterValidation(control: FormControl) {
    return ((control.value as string) || '').match(/[$-/:-?{-~!"^_`\[\]]/) !=
      null
      ? null
      : { specialcharacters: true };
  }

  public noUnAllowedCharactersValidation(control: FormControl) {
    return ((control.value as string) || '').match(
      /[^a-zA-Z0-9$ -/@:-?{-~!"^_`\[\]]/,
    ) == null
      ? null
      : { unallowedcharacters: true };
  }

  public firstAndLastNameValidation(control: FormControl) {
    return ((control.value as string) || '').match(
      /^[A-Z](([',. -][A-Z])?[a-z]*)*$/,
    ) != null
      ? null
      : { firstandlastname: true };
  }

  public minPasswordLenghtValidation(control: FormControl) {
    return ((control.value as string) || '')?.length >= 8
      ? null
      : { minpasswordlenght: true };
  }

  public maxPasswordLenghtValidation(control: FormControl) {
    return ((control.value as string) || '')?.length <= 32
      ? null
      : { maxpasswordlenght: true };
  }

  public passwordsMatch(
    control: AbstractControl,
  ): { invalid: boolean; mismatch: boolean } | null {
    if (
      control.get('userPassword')?.dirty &&
      control.get('userPasswordConfirmation')?.dirty &&
      control.get('userPassword')?.value !=
        control.get('userPasswordConfirmation')?.value
    ) {
      return { invalid: true, mismatch: true };
    }
    return null;
  }

  public birthDateValidation(control: FormControl) {
    if (control.value != null) {
      let enterdDate: Date = new Date(control.value);
      return moment(enterdDate) < moment(new Date()).subtract(16, 'year') &&
        moment(enterdDate) > moment(new Date()).subtract(100, 'year')
        ? null
        : { datevalidate: true };
    }
    return null;
  }

  public markFormControlsAsDirty(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsDirty();
    });
  }

  userEmailValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      return this.httpClientService
        .getRequest(`/Users/Email/${control.value}`)
        .pipe(
          takeUntil(this.unsubscribe$),
          map((res) => {
            if (res) {
              this.notificationService.showErrorMessage(
                'Someone\'s already using that email.',
              );
              return { userEmailExists: true };
            }
            return null;
          }),
          catchError((err) => {
            this.notificationService.showErrorMessage(
              'Email uniqueness check error.',
            );
            return of([{ userEmailExists: true }]);
          }),
        );
    };
  }
}
