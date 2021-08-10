import { User } from '../user';

export interface UserRegisterDto extends User {
  password: string;
}
