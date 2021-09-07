import { Role } from './role';

export interface UserTableData {
  id?: string;
  firstName: string;
  lastName: string;
  birthDate: Date;
  creationDate: Date;
  email: string;
  avatarUrl?:string;
  roles?:Role[];
  phone?:string;
  skype?:string;
  slack?:string;

  isEmailConfirmed: boolean;
  isFollowed?: boolean;
}