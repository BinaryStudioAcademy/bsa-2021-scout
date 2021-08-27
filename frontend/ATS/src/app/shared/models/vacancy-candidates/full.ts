import { ShortCandidateReview } from '../candidate-reviews/short';
import { CandidateToStageHistory } from '../candidate-to-stages/history';
import { Model } from '../model';

export interface FullVacancyCandidate extends Model {
  hrWhoAddedId: string;
  hrWhoAddedFullName: string;
  currentStageName?: string;
  fullName: string;
  email: string;
  phone: string;
  cvLink?: string;
  cvName?: string;
  experience: number;
  experienceDescription: string;
  comments: string;
  stagesHistory: CandidateToStageHistory[];
  reviews: ShortCandidateReview[];
  dateAdded: Date;
}
