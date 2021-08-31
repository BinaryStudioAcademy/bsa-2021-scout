import { Model } from '../model';
import { ApplicantRecentActivity } from './applicant-recent-activity';

export interface VacancyWithRecentActivity extends Model {
  title: string;
  projectName: string;
  activity: ApplicantRecentActivity[];
}
