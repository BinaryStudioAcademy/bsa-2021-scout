import { ElasticEntity } from '../elastic-entity/elastic-entity';

export class CreateApplicant {
  firstName: string='';
  lastName: string='';
  email: string='';
  phone: string='';
  skype: string='';
  linkedInUrl: string='';
  experience: number=0;
  experienceDescription?: string;
  skills?: string;

  tags: ElasticEntity = new ElasticEntity();

  cv: File | null = null;
}
