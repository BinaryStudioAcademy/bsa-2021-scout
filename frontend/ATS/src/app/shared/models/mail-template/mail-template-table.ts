export interface MailTemplateTable{  
  id: string;
  title: string;
  subject: string;
  html: string;
  visibilitySetting : string;
  userCreated: string;
  dateCreation: Date;
  attachmentsCount: number;
  isFollowed: boolean;
}