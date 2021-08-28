import { OnlyApplicant } from '../applicants/only-applicant';
import { Model } from '../model';

export interface ShortVacancyCandidateWithApplicant extends Model {
  averageMark?: number;
  dateAdded: Date;
  applicant: OnlyApplicant;
  isSelfApplied: boolean;
  isViewed: boolean;
}
