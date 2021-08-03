import { Component, Inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { applicantGroup } from '../../validators/applicant-validator';
import { FullApplicant } from '../applicants/applicants.component';

@Component({
  selector: 'app-update-applicant',
  templateUrl: 'update-applicant.component.html',
  styleUrls: [ 'update-applicant.component.scss' ],
})

export class UpdateApplicantComponent {
  public validationGroup: FormGroup|undefined = undefined;
    
  constructor(@Inject(MAT_DIALOG_DATA) public applicant: FullApplicant) {
    this.validationGroup = applicantGroup;
  }
}