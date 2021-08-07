import { Model } from '../model';
import { VacancyCandidateWithApplicant } from '../vacancy-candidates/with-applicant';
import { StageType } from './type';

export interface StageWithCandidates extends Model {
  name: string;
  type: StageType;
  index: number;
  isReviewable: boolean;
  vacancyId: string;
  candidates: VacancyCandidateWithApplicant[];
}
