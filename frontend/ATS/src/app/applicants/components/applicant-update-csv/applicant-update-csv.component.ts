import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { CsvApplicant } from '../../models/CsvApplicant';

@Component({
  selector: 'app-applicant-update-csv',
  templateUrl: './applicant-update-csv.component.html',
  styleUrls: ['./applicant-update-csv.component.scss', '../../common/common.scss'],
})
export class ApplicantUpdateCsvComponent {
  public validationGroup: FormGroup | undefined = new FormGroup({
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
    experience: new FormControl(''),
    phone: new FormControl('', [
      Validators.required,
      Validators.pattern('^\\+?[0-9]{8,16}'),
    ]),
    linkedInUrl: new FormControl('', [
      Validators.pattern('^https:\\/\\/www.linkedin.com\\/[a-z0-9\\-]+'),
    ]),
  });

  public updatedApplicant: CsvApplicant = new CsvApplicant();
  public applicant: CsvApplicant = new CsvApplicant();

  constructor(@Inject(MAT_DIALOG_DATA) applicant: CsvApplicant,
    public dialogRef: MatDialogRef<ApplicantUpdateCsvComponent>) {
    this.updatedApplicant = new CsvApplicant(applicant);
    this.applicant = applicant;
  }

  public updateApplicant(): void {
    this.updatedApplicant.isValid = true;
    this.dialogRef.close({ data: this.updatedApplicant });
  }
}


