import { MailAttachment } from '../mail-attachment/mail-attachment';

export class MailTemplate{  
  id : string = '';
  slug : string = '';
  subject : string = '';
  html : string = '';
  visibilitySetting : number = 0;
  mailAttachments : MailAttachment[] = [];
}