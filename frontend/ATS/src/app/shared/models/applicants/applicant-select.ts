import { Applicant } from '../applicants/applicant';

export interface ApplicantIsSelected extends Applicant {
  isSelected?: boolean;
  isShowAllTags?: boolean;  
}

