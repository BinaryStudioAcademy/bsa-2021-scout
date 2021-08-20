import { User } from 'src/app/users/models/user';
import { Model } from '../model';
import { VacancyStatus } from './vacancy-status';

export interface VacancyData extends Model{
  title: string;
  currentApplicantsAmount: number;
  requiredCandidatesAmount: number;
  department: string;
  responsibleHr: User;
  creationDate: Date;
  status: VacancyStatus;
}
  ​