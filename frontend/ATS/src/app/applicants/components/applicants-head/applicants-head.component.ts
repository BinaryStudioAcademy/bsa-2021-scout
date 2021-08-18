import { Component, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
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

  constructor(
    private readonly dialog: MatDialog,
  ) {}

  public applySearchValue(): void {
    this.search.emit(this.searchValue);
  }

  public clearSearchInput(): void {
    this.searchValue = '';
    this.search.emit(this.searchValue);
  }

  public showApplicantsCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateApplicantComponent, {
      width: '532px',
      autoFocus: false,
    });

    this.applicantCreated.emit(dialogRef.afterClosed());
  }
}
