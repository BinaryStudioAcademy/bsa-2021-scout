import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmailTemplatesListComponent } from
  './components/email-templates-list/email-templates-list.component';
import { EmailTemplateAddComponent } from
  './components/email-template-add/email-template-add.component';
import { EmailTemplateEditComponent } from
  './components/email-template-edit/email-template-edit.component';
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



@NgModule({
  declarations: [
    EmailTemplatesListComponent,
    EmailTemplateAddComponent,
    EmailTemplateEditComponent,
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
export class EmailTemplatesModule { }
