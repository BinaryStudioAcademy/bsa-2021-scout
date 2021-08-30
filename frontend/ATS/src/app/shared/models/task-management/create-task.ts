import { Model } from '../model';
import { ApplicantShort } from './applicant-short';
import { UserInfo } from './user-short';

export interface CreateTask extends Model {
  name: string;
  applicant : ApplicantShort;
  dueDate: Date;
  doneDate?: Date;  
  isDone: boolean;
  note?: string;
  createdBy: UserInfo;
  createdDate: Date;
  teamMembers: UserInfo[];
}