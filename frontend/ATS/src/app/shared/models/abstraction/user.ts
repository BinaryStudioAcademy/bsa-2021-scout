import { Model } from '../model';

export interface User extends Model{
  firstName: string,
  lastName: string,
  middleName: string,
  birthDate: Date,
  email:string
}