import { ProjectInfo } from './../../../projects/models/project-info';
import { User } from 'src/app/users/models/user';
import { Model } from '../model';
import { VacancyStatus } from './vacancy-status';

export interface VacancyData extends Model{
  title: string;
  position: number;
  isFollowed: boolean;
  candidatesAmount: number;
  responsibleHr: User;
  creationDate: Date;
  project: ProjectInfo,
  status: VacancyStatus;
}
  ​