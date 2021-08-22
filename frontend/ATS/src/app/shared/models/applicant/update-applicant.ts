import { ElasticEntity } from '../elastic-entity/elastic-entity';

export interface UpdateApplicant {
  id: string;
  firstName: string;
  lastName: string;
  middleName: string;
  email: string;
  phone: string;
  skype: string;
  linkedInUrl: string;
  experience: number;
  hasCv: boolean;
  cv: File | null;

  tags: ElasticEntity;
}
