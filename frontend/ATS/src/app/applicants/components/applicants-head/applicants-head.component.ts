import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { CreateApplicant } from 'src/app/shared/models/applicants/create-applicant';
import { ApplicantsUploadCsvComponent } 
  from '../applicants-upload-csv/applicants-upload-csv.component';
import { CreateApplicantComponent } from '../create-applicant/create-applicant.component';

@Component({
  selector: 'app-applicants-head',
  templateUrl: './applicants-head.component.html',
  styleUrls: ['./applicants-head.component.scss'],
})
export class ApplicantsHeadComponent implements OnInit{
  public searchValue = '';
  public page: string = 'all';
  public creationData?: CreateApplicant;

  @Output() public search = new EventEmitter<string>();
  @Output() public applicantCreated = new EventEmitter<Observable<Applicant>>();
  @Output() public applicantsFileUploaded = new EventEmitter<void>();
  @Output() public togglePage = new EventEmitter<string>();

  constructor(
    private readonly dialog: MatDialog,
    private readonly route: ActivatedRoute,
  ) {}
  private readonly applicantPageToken: string = 'applicantPageToken';
  public ngOnInit(): void {
    this.route.queryParams.subscribe((query) => {
      if (query['data']) {
        const creationData: CreateApplicant = JSON.parse(atob(query['data']));
        this.creationData = creationData;
        this.showApplicantsCreateDialog();
      }
    });
    this.page = localStorage.getItem(this.applicantPageToken) ? 
      localStorage.getItem(this.applicantPageToken)! : 'all';
  }

  public applySearchValue(): void {
    this.search.emit(this.searchValue);
  }

  public showApplicantsCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateApplicantComponent, {
      width: '532px',
      height: '95vh',
      autoFocus: false,
      data: this.creationData,
    });

    this.applicantCreated.emit(dialogRef.afterClosed());
  }

  public showUploadCSVDialog(): void{
    const dialogRef = this.dialog.open(ApplicantsUploadCsvComponent, {
      width: '600px',
      panelClass: 'applicants-csv-modal',
      autoFocus: false,
    })
      .afterClosed()
      .subscribe(_=>this.applicantsFileUploaded.emit());
  }

  public toggleFollowedOrAll(page: string): void {
    this.page = page;
    this.togglePage.emit(page);
  }
}
