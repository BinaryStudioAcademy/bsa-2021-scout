import { ElasticEntity } from '../elastic-entity/elastic-entity';
import { ApplicantVacancyInfo } from './applicant-vacancy-info';

export interface Applicant {
  id: string;
  firstName: string;
  lastName: string;
  middleName: string;
  linkedInUrl: string;
  email: string;
  phone: string;
  skype: string;
  experience: number;
  hasCv: boolean;

  tags: ElasticEntity;
  vacancies: ApplicantVacancyInfo[];  
}


