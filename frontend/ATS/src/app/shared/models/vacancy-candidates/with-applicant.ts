import { Applicant } from '../applicants/applicant';
import { Model } from '../model';

export interface VacancyCandidateWithApplicant extends Model {
  firstContactDate?: Date;
  secondContactDate?: Date;
  thirdContactDate?: Date;
  salaryExpectation: number;
  contactedBy: string;
  comments: string;
  experience: number;
  stageId: string;
  applicant: Applicant;
}
