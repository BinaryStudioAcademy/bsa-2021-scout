import { Model } from '../model';
import { Review } from '../reviews/review';
import { ShortVacancyCandidateWithApplicant } from '../vacancy-candidates/short-with-applicant';
import { StageType } from './type';

export interface StageWithCandidates extends Model {
  name: string;
  type: StageType;
  index: number;
  isReviewable: boolean;
  vacancyId: string;
  candidates: ShortVacancyCandidateWithApplicant[];
  reviews: Review[];
}
