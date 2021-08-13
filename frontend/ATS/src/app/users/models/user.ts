import { Role } from './role';

export interface User {
  id?: string;
  firstName: string;
  lastName: string;
  middleName: string;
  birthDate: Date;
  email: string;
  roles: Role[];
}