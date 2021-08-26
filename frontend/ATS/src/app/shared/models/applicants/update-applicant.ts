import { ElasticEntity } from '../elastic-entity/elastic-entity';

export interface UpdateApplicant {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  skype: string;
  linkedInUrl: string;
  experience: number;
  experienceDescription?: string;
  skills?: string;
  hasCv: boolean;
  cv: File | null;

  tags: ElasticEntity;
}
