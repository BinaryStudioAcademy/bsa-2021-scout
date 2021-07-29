import { Model } from '../model';
import { CandidateStatus } from './status';

export interface ShortCandidate extends Model {
  avatar: string;
  fullName: string;
  status: CandidateStatus;
  appliedAt: Date;
}
