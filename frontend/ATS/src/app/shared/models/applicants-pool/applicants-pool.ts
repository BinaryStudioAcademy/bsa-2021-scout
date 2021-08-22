import { ApplicantIsSelected } from '../applicant/applicant-select';
import { Model } from '../model';

export interface ApplicantsPool extends Model{
  name: string;
  position: number;
  description: string;
  createdBy: string;
  dateCreated: Date;
  applicants: ApplicantIsSelected [];
  count: number;
}