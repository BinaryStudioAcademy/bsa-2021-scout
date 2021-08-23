import { Component, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { ApplicantsUploadCsvComponent } 
  from '../applicants-upload-csv/applicants-upload-csv.component';
import { CreateApplicantComponent } from '../create-applicant/create-applicant.component';

@Component({
  selector: 'app-applicants-head',
  templateUrl: './applicants-head.component.html',
  styleUrls: ['./applicants-head.component.scss'],
})
export class ApplicantsHeadComponent {
  public searchValue = '';

  @Output() public search = new EventEmitter<string>();
  @Output() public applicantCreated = new EventEmitter<Observable<Applicant>>();
  @Output() public applicantsFileUploaded = new EventEmitter<void>();

  constructor(private readonly dialog: MatDialog) {}

  public applySearchValue(): void {
    this.search.emit(this.searchValue);
  }

  public showApplicantsCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateApplicantComponent, {
      width: '532px',
      height: '95vh',
      autoFocus: false,
    });

    this.applicantCreated.emit(dialogRef.afterClosed());
  }

  public showUploadCSVDialog(): void{
    const dialogRef = this.dialog.open(ApplicantsUploadCsvComponent, {
      width: '532px',
      height: '38vh',
      panelClass: 'applicants-csv-modal',
      autoFocus: false,
    })
      .afterClosed()
      .subscribe(_=>this.applicantsFileUploaded.emit());
  }
}
