import { User } from 'src/app/users/models/user';

export class CsvApplicant {
  firstName: string='';
  lastName: string='';
  linkedInUrl: string='';
  email: string='';
  phone: string='';
  skype: string='';
  experience: number=0;
  isValid: boolean=false;
  isExist: boolean=false;
  isRepeat: boolean=false;
  isAdded: boolean=false;
  creationDate: Date = new Date();
  user: User | null = null;
  
  constructor(applicant?: CsvApplicant){
    if(applicant){
      this.firstName=applicant.firstName;
      this.lastName=applicant.lastName;
      this.linkedInUrl=applicant.linkedInUrl;
      this.email=applicant.email;
      this.phone=applicant.phone;
      this.skype=applicant.skype;
      this.experience=applicant.experience;
      this.isValid=applicant.isValid;
      this.isExist=applicant.isExist;
      this.isRepeat=applicant.isRepeat;
    }
  }
}