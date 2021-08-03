import { FormControl, FormGroup, Validators } from '@angular/forms';

export const applicantGroup = new FormGroup({
  'firstName': new FormControl('', [
    Validators.required,
  ]),
  'lastName': new FormControl('', [
    Validators.required,
  ]),
  'middleName': new FormControl('', [
    Validators.required,
  ]),
  'email': new FormControl('', [
    Validators.required,
    Validators.email,
    Validators.pattern('\\S{1,}@\\S{3,}'),
  ]),
  'experience': new FormControl('', [
    Validators.required,
    Validators.min(0),
  ]),
  'phone': new FormControl('', [
    Validators.required,
  ]),
  'skype': new FormControl('', [
    Validators.required,
  ]),
});