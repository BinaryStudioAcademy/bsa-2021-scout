import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { applicantGroup } from '../../validators/applicant-validator';

@Component({
  selector: 'app-create-applicant',
  templateUrl: 'create-applicant.component.html',
  styleUrls: [ 'create-applicant.component.scss' ],
})

export class CreateApplicantComponent {
  public validationGroup: FormGroup|undefined = undefined;

  constructor() {
    this.validationGroup = applicantGroup;
  }
}