import { User } from 'src/app/users/models/user';

export class CsvFile {
  id: string = '';
  name: string = '';
  json: string = '';
  dateAdded: Date = new Date();
  user: User | null = null;
}