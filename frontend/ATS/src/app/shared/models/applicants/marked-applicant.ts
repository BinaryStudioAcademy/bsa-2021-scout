import { GetShortApplicant } from 'src/app/shared/models/applicants/get-short-applicant';

export class MarkedApplicant {
  id: string='';
  firstName: string='';
  lastName: string='';
  isApplied: boolean=false;

  constructor(applicant: GetShortApplicant){
    this.id = applicant.id;
    this.firstName = applicant.firstName;
    this.lastName = applicant.lastName;
    this.isApplied = false;
  }
}