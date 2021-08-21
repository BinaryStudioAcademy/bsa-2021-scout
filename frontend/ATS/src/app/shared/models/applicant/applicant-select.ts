import { Applicant } from '../applicant/applicant';

export interface ApplicantIsSelected extends Applicant {
  isSelected?:boolean;
}

