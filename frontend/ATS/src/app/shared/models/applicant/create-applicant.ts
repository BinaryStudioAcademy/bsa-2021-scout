import { ElasticEntity } from '../elastic-entity/elastic-entity';

export interface CreateApplicant {
  firstName: string;
  lastName: string;
  middleName: string;
  email: string;
  phone: string;
  skype: string;
  linkedInUrl: string;
  experience: number;

  tags: ElasticEntity;
}
