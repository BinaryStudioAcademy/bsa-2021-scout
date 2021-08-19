import { User } from 'src/app/users/models/user';
import { VacancyStatus } from './vacancy-status';

export interface VacancyData {
  title: string;
  candidatesAmount: number;
  department: string;
  responsibleHr: User;
  creationDate: Date;
  status: VacancyStatus;
}
  ​