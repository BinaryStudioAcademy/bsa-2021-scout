import { Model } from '../model';

export interface RecentActivity extends Model {
  moverId: string;
  moverName: string;
  candidateId: string;
  candidateName: string;
  stageId: string;
  stageName: string;
  vacancyId: string;
  vacancyName: string;
  dateAdded: Date;
}
