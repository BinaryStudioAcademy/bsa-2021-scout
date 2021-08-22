import { Applicant } from './applicant';

export interface ViewableApplicant extends Applicant {
  isShowAllTags: boolean;
}
