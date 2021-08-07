import { Model } from '../model';

export interface VacancyCandidate extends Model {
  firstContactDate?: Date;
  secondContactDate?: Date;
  thirdContactDate?: Date;
  salaryExpectation: number;
  contactedBy: string;
  comments: string;
  experience: number;
  stageId: string;
  applicantId: string;
}
