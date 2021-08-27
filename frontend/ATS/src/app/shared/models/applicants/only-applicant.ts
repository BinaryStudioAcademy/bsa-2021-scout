import { Model } from '../model';

export interface OnlyApplicant extends Model {
  firstName: string;
  lastName: string;
  birthDate: Date;
  email: string;
  phone: string;
  skype: string;
  experience: string;
  experienceDescription?: string;
  skills?: string;
  toBeContacted: Date;
  companyId: string;
}
