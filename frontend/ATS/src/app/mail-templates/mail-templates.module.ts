import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MailTemplatesListComponent } from
  './components/mail-templates-list/mail-templates-list.component';
import { MailTemplateAddComponent } from
  './components/mail-template-add/mail-template-add.component';
import { MailTemplateEditComponent } from
  './components/mail-template-edit/mail-template-edit.component';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { SharedModule } from '../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule} from '@angular/common/http';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { DxHtmlEditorModule, DxPopupModule } from 'devextreme-angular';
import { SelectTemplateEntitiesComponent } 
  from './components/select-template-entities/select-template-entities.component';

@NgModule({
  declarations: [
    MailTemplatesListComponent,
    MailTemplateAddComponent,
    MailTemplateEditComponent,
    SelectTemplateEntitiesComponent,
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatToolbarModule,
    MatIconModule,
    MatPaginatorModule,
    SharedModule,
    MatDialogModule,
    MatSelectModule,
    ReactiveFormsModule,
    HttpClientModule, 
    AngularEditorModule,
    DxHtmlEditorModule, 
    DxPopupModule,
  ],
  exports:[
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatToolbarModule,
    MatIconModule,
    MatPaginatorModule,
  ],
})
export class MailTemplatesModule { }
