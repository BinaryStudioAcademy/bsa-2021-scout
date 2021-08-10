import { VacancyStatus } from './vacancy-status';

export interface VacancyData {
  title: string;
  currentApplicantsAmount: number;
  requiredCandidatesAmount: number;
  department: string;
  responsible: string;
  creationDate: Date;
  status: VacancyStatus;
}
  ​