import { Role } from './role';

export interface User {
  id?: string;
  firstName: string;
  lastName: string;
  birthDate: Date;
  creationDate: Date;
  email: string;
  isEmailConfirmed: boolean;
  roles?: Role[];

  avatarUrl?: string,
  skype?: string,
  slack?: string,
  phone?:string
}