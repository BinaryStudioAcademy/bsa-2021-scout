
export interface UserTableData {
  id?: string;
  firstName: string;
  lastName: string;
  birthDate: Date;
  creationDate: Date;
  email: string;
  isEmailConfirmed: boolean;
  isFollowed?: boolean;
}