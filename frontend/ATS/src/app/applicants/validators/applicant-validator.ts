import { FormControl, FormGroup, Validators } from '@angular/forms';

export const applicantGroup = new FormGroup({
  'firstName': new FormControl('', [
    Validators.required,
    Validators.pattern('[a-zA-Z]+'),
  ]),
  'lastName': new FormControl('', [
    Validators.required,
    Validators.pattern('[a-zA-Z]+'),
  ]),
  'middleName': new FormControl('', [
    Validators.required,
    Validators.pattern('[a-zA-Z]+'),
  ]),
  'email': new FormControl('', [
    Validators.required,
    Validators.email,
    Validators.pattern('\\S{1,}@\\S{3,}'),
  ]),
  'experience': new FormControl('', [
    Validators.required,
  ]),
  'phone': new FormControl('', [
    Validators.required,
    Validators.pattern('[0-9]{8,16}'),
  ]),
  'skype': new FormControl('', [
    Validators.required,
    Validators.pattern('\\S{6,32}'),
  ]),
});