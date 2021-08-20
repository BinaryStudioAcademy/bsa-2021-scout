import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ApplicantsComponent } from './components/applicants/applicants.component';
import { CreateApplicantComponent } from './components/create-applicant/create-applicant.component';
import { UpdateApplicantComponent } from './components/update-applicant/update-applicant.component';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { ApplicantsHeadComponent } from './components/applicants-head/applicants-head.component';
import { MatChipsModule } from '@angular/material/chips';
import { TagsEditComponent } from './components/tags-edit/tags-edit.component';
import { ApplicantControlComponent } 
  from './components/applicant-control/applicant-control.component';

@NgModule({
  declarations: [
    ApplicantsComponent,
    CreateApplicantComponent,
    UpdateApplicantComponent,
    ApplicantsHeadComponent,
    TagsEditComponent,
    ApplicantControlComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatChipsModule,
  ],
  providers: [],
  bootstrap: [ApplicantsComponent],
  exports: [ApplicantsComponent],
})
export class ApplicantsModule {}