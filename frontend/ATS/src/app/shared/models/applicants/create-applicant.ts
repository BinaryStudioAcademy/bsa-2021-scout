import { CsvApplicant } from 'src/app/applicants/models/CsvApplicant';
import { ElasticEntity } from '../elastic-entity/elastic-entity';

export class CreateApplicant {
  firstName: string = '';
  lastName: string = '';
  email: string = '';
  phone: string = '';
  linkedInUrl: string = '';
  experience: number = 0;
  experienceDescription?: string;
  skills?: string;
  tags: ElasticEntity = new ElasticEntity();
  cv: string | File | null = null;
  photo: string | File | null = null;

  public constructor(csvApplicant: CsvApplicant | null) {
    if (csvApplicant){
      this.firstName = csvApplicant.firstName;
      this.lastName = csvApplicant.lastName;
      this.email = csvApplicant.email;
      this.phone = csvApplicant.phone;
      this.linkedInUrl = csvApplicant.linkedInUrl;
      this.experience = csvApplicant.experience;
    }
  }
}
