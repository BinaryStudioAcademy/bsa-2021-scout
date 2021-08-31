import { FormControl, FormGroup, Validators } from '@angular/forms';

export const applicantGroup = new FormGroup({
  firstName: new FormControl('', [
    Validators.required,
    Validators.pattern('^[A-Z]{1}[a-z]+([\\s-]{1}[A-Z]{1}[a-z]+)?'),
  ]),
  lastName: new FormControl('', [
    Validators.required,
    Validators.pattern('^[A-Z]{1}[a-z]+([\\s-]{1}[A-Z]{1}[a-z]+)?'),
  ]),
  email: new FormControl('', [
    Validators.required,
    Validators.email,
    Validators.pattern('^\\S{1,}@\\S{3,}\\.[a-z]+'),
  ]),
  experienceDescription: new FormControl('', [Validators.required]),
  experience: new FormControl('', [Validators.required]),
  phone: new FormControl('', [
    Validators.required,
    Validators.pattern('^\\+?[0-9]{8,16}'),
  ]),
  skills: new FormControl('', [Validators.required]),
  skype: new FormControl('', [
    Validators.pattern(/^https:\/\/(www\.)?skype\.com\/\S{6,32}/g),
  ]),
  linkedInUrl: new FormControl('', [
    Validators.pattern(/^https:\/\/(www\.)?linkedin\.com\/in\/[a-z0-9\-]+\/?$/g),
  ]),
});
