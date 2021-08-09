import { VacancyStatus } from './vacancy-status';

export interface VacancyData {
  title: string;
  current_applicants_amount: number;
  required_candidates_amount: number;
  department: string;
  responsible: string;
  creationDate: Date;
  status: VacancyStatus;
}
  ​