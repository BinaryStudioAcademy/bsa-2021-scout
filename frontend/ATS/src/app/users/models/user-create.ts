export interface UserCreate{
  firstName: string;
  lastName: string;
  birthDate: Date;
  email: string;
  image?: string,
  skype?: string,
  phone?:string
}