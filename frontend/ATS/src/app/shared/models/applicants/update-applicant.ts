import { ElasticEntity } from '../elastic-entity/elastic-entity';

export interface UpdateApplicant {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  linkedInUrl: string;
  experience: number;
  experienceDescription?: string;
  creationDate: Date;
  skills?: string;
  hasCv: boolean;
  hasPhoto: boolean;
  cv: string | File | null;
  photo: string | File | null;

  tags: ElasticEntity;
}
