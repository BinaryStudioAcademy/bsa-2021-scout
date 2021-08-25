import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmailTemplatesListComponent } from
  './components/email-templates-list/email-templates-list.component';
import { EmailTemplateAddComponent } from
  './components/email-template-add/email-template-add.component';
import { EmailTemplateEditComponent } from
  './components/email-template-edit/email-template-edit.component';



@NgModule({
  declarations: [
    EmailTemplatesListComponent,
    EmailTemplateAddComponent,
    EmailTemplateEditComponent,
  ],
  imports: [
    CommonModule,
  ],
})
export class EmailTemplatesModule { }
