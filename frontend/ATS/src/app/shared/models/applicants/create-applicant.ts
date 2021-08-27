import { ElasticEntity } from '../elastic-entity/elastic-entity';

export class CreateApplicant {
  firstName: string = '';
  lastName: string = '';
  middleName: string = '';
  email: string = '';
  phone: string = '';
  skype: string = '';
  linkedInUrl: string = '';
  experience: number = 0;

  tags: ElasticEntity = new ElasticEntity();

  cv: File | null = null;
}
