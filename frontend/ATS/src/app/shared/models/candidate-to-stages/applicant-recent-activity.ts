import { Model } from '../model';

export interface ApplicantRecentActivity extends Model {
  moverId: string;
  moverName: string;
  stageId: string;
  stageName: string;
  dateAdded: Date;
}
