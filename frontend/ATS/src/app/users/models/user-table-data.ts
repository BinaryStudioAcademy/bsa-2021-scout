
export interface UserTableData {
  id?: string;
  firstName: string;
  lastName: string;
  birthDate: Date;
  creationDate: Date;
  email: string;
  avatarUrl?:string;
  phone:string;
  skype:string;
  isEmailConfirmed: boolean;
  isFollowed?: boolean;
}