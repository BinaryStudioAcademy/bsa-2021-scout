import { Model } from '../model';

export interface UserInfo extends Model {
  firstName: string;
  lastName: string;
  image?: string;  
}