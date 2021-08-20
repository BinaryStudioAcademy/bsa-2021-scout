import { Applicant } from '../applicant/applicant';

export interface ViewableApplicant extends Applicant {
  isShowAllTags: boolean
};