import { ElasticEntity } from '../elastic-entity/elastic-entity';

export interface CreateApplicant {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  skype: string;
  linkedInUrl: string;
  experience: number;
  experienceDescription?: string;
  skills?: string;

  tags: ElasticEntity;

  cv: File | null;
}
