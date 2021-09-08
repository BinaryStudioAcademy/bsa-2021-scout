import { ApplicantIsSelected } from '../applicants/applicant-select';
import { Model } from '../model';

export interface ApplicantsPool extends Model{
  name: string;
  position: number;
  description: string;
  createdBy: string;
  isFollowed: boolean;
  dateCreated: Date;
  applicants: ApplicantIsSelected [];
  count: number;
}