import { Applicant } from '../applicants/applicant';
import { Model } from '../model';

export interface ApplicantsPool extends Model{
  name: string;
  position: number;
  description?: string;
  createdBy: string;
  dateCreated: Date;
  applicants: Applicant [];
}