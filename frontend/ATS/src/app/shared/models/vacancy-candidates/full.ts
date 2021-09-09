import { ShortCandidateReview } from '../candidate-reviews/short';
import { CandidateToStageHistory } from '../candidate-to-stages/history';
import { Model } from '../model';
import { Tag } from '../tags/tag';

export interface FullVacancyCandidate extends Model {
  hrWhoAddedId: string;
  hrWhoAddedFullName: string;
  currentStageName?: string;
  fullName: string;
  email: string;
  phone: string;
  cvLink?: string;
  cvName?: string;
  photoLink?: string;
  experience: number;
  experienceDescription: string;
  comments: string;
  tags: Tag[];
  stagesHistory: CandidateToStageHistory[];
  reviews: ShortCandidateReview[];
  dateAdded: Date;
  isSelfApplied: boolean;
}
