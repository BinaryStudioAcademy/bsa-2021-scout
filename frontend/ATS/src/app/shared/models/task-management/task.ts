import { Model } from '../model';
import { ApplicantShort } from './applicant-short';
import { UserInfo } from './user-short';

export interface Task extends Model {
  name: string;
  applicant : ApplicantShort;
  dueDate: Date;
  doneDate?: Date;  
  isDone: boolean;
  isReviewed: boolean;
  note?: string;
  createdBy: UserInfo;
  dateCreated: Date;
  teamMembers: UserInfo[];
}