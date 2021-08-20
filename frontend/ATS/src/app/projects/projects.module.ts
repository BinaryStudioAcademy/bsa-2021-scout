import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectsListComponent } from './components/projects-list/projects-list.component';
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
import { ProjectsAddComponent } from './components/projects-add/projects-add.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ProjectsEditComponent } from './components/projects-edit/projects-edit.component';

@NgModule({
  declarations: [
    ProjectsListComponent,
    ProjectsAddComponent,
    ProjectsEditComponent,
  ],
  imports:[
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
export class ProjectsModule {}
