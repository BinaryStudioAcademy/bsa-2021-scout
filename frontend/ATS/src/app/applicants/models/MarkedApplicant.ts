import { GetApplicant } from 'src/app/shared/models/applicants/get-applicant';

export class MarkedApplicant {
  id: string='';
  firstName: string='';
  lastName: string='';
  linkedInUrl: string='';
  email: string='';
  phone: string='';
  skype: string='';
  experience: number=0;
  isApplied: boolean=false;

  constructor(applicant: GetApplicant){
    this.id = applicant.id;
    this.firstName = applicant.firstName;
    this.lastName = applicant.lastName;
    this.linkedInUrl = applicant.linkedInUrl;
    this.email = applicant.email;
    this.phone = applicant.phone;
    this.skype = applicant.skype;
    this.experience = applicant.experience;
    this.isApplied = false;
  }
  
}