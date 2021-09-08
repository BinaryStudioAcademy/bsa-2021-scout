
export interface UserCreate{
  id?:string;
  firstName: string;
  lastName: string;
  birthDate: Date;
  email: string;
  avatar?: File,
  skype?: string,
  phone?:string,
  slack?:string,
  isImageToDelete:boolean

}