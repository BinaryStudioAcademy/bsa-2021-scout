import { CsvApplicant } from 'src/app/applicants/models/CsvApplicant';
import { ElasticEntity } from '../elastic-entity/elastic-entity';

export class CreateApplicant {
  firstName: string = '';
  lastName: string = '';
  email: string = '';
  phone: string = '';
  skype: string = '';
  linkedInUrl: string = '';
  experience: number = 0;
  experienceDescription?: string;
  skills?: string;

  tags: ElasticEntity = new ElasticEntity();

  cv: File | null = null;

  constructor(csvApplicant: CsvApplicant) {
    this.firstName = csvApplicant.firstName;
    this.lastName = csvApplicant.lastName;
    this.email = csvApplicant.email;
    this.phone = csvApplicant.phone;
    this.skype = csvApplicant.skype;
    this.linkedInUrl = csvApplicant.linkedInUrl;
    this.experience = csvApplicant.experience;
  }
}
