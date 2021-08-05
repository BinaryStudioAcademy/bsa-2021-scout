import { ShortCandidateReview } from '../candidate-reviews/short';
import { Model } from '../model';

export interface FullVacancyCandidate extends Model {
  hrWhoAddedFullName: string;
  currentStageName?: string;
  fullName: string;
  email: string;
  phone: string;
  cv?: string;
  experience: number;
  comments: string;
  reviews: ShortCandidateReview[];
  dateAdded: Date;
}
