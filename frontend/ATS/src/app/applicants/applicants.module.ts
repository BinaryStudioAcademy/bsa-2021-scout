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
import { ApplicantControlComponent } 
  from './components/applicant-control/applicant-control.component';
import { ApplicantsUploadCsvComponent } 
  from './components/CsvComponents/applicants-upload-csv/applicants-upload-csv.component';
import { ApplicantUpdateCsvComponent } 
  from './components/applicant-update-csv/applicant-update-csv.component';
import { ApplicantFileTablesComponent } 
  from './components/CsvComponents/applicant-csv-file-tables/applicant-csv-file-tables.component';
import { SelfApplyingComponent } from './components/self-applying/self-applying.component';
import { ApplicantHistoryComponent }
  from './components/applicant-history/applicant-history.component';
import { ApplicantsRoutingModule } from './applicants-routing.module';
/* eslint-disable max-len */
import { ApplicantCsvFilesListComponent } from './components/CsvComponents/applicant-csv-files-list/applicant-csv-files-list.component';
import { CsvFileComponent } from './components/CsvComponents/one-csv-file/one-csv-file.component';
import { CreateApplicantFromVariantsComponent }
  from './components/create-applicant-from-variants/create-applicant-from-variants.component';
import { StartCvParsingModalComponent }  
  from './components/start-cv-parsing-modal/start-cv-parsing-modal.component';
import { CvParsingStartedModalComponent }
  from './components/cv-parsing-started-modal/cv-parsing-started-modal.component';

@NgModule({
  declarations: [
    ApplicantsComponent,
    CreateApplicantComponent,
    UpdateApplicantComponent,
    ApplicantsHeadComponent,
    ApplicantControlComponent,
    ApplicantsUploadCsvComponent,
    ApplicantUpdateCsvComponent,
    ApplicantFileTablesComponent,
    SelfApplyingComponent,
    ApplicantHistoryComponent,
    ApplicantCsvFilesListComponent,
    CsvFileComponent,
    CreateApplicantFromVariantsComponent,
    StartCvParsingModalComponent,
    CvParsingStartedModalComponent,
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
    ApplicantsRoutingModule,
  ],
  providers: [],
  bootstrap: [ApplicantsComponent],
  exports: [ApplicantsComponent],
})
export class ApplicantsModule {}