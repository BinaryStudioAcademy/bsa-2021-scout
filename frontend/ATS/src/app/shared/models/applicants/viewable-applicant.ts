import { Applicant } from './applicant';

export interface ViewableApplicant extends Applicant {
  position: number;
  isShowAllTags: boolean;
  isFollowed: boolean;
  isSelfApplied: boolean;
}
