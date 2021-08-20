import { Model } from '../model';

export interface Applicant extends Model {
  firstName: string;
  lastName: string;
  middleName?: string;
  birthDate?: Date;
  email: string;
  phone?: string;
  skype?: string;
  experience?: string;
  toBeContacted?: Date;
  companyId?: string;
  isSelected?: boolean;
}
