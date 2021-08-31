import { ElasticEntity } from '../elastic-entity/elastic-entity';
import { ApplicantVacancyInfo } from './applicant-vacancy-info';

export interface Applicant {
  id: string;
  firstName: string;
  lastName: string;
  birthDate?: Date;
  email: string;
  phone?: string;
  skype?: string;
  linkedInUrl?: string;
  experience?: number;
  experienceDescription?: string;
  skills?: string;
  toBeContacted?: Date;
  companyId?: string;
  isSelected?: boolean;
  hasCv: boolean;
  creationDate: Date;

  tags: ElasticEntity;
  vacancies: ApplicantVacancyInfo[];
}
