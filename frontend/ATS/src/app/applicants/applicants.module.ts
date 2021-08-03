import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ApplicantsComponent } from './components/applicants/applicants.component';
import { SharedModule } from '../shared/shared.module';
import { CreateApplicantComponent } from './components/create-applicant/create-applicant.component';
import { UpdateApplicantComponent } from './components/update-applicant/update-applicant.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    ApplicantsComponent,
    CreateApplicantComponent,
    UpdateApplicantComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [ApplicantsComponent],
  exports: [ApplicantsComponent],
})
export class ApplicantsModule {}