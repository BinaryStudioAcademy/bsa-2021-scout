import { GetApplicant } from 'src/app/shared/models/applicant/get-applicant';

export class MarkedApplicant {
  id: string='';
  firstName: string='';
  lastName: string='';
  middleName: string='';
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
    this.middleName = applicant.middleName;
    this.linkedInUrl = applicant.linkedInUrl;
    this.email = applicant.email;
    this.phone = applicant.phone;
    this.skype = applicant.skype;
    this.experience = applicant.experience;
    this.isApplied = false;
  }
  
}