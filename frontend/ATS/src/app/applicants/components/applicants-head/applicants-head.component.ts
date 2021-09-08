import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { CreateApplicant } from 'src/app/shared/models/applicants/create-applicant';
import { ApplicantCreationVariants } from 'src/app/shared/models/applicants/creation-variants';
import { CreateApplicantFromVariantsComponent }
  from '../create-applicant-from-variants/create-applicant-from-variants.component';
import { CreateApplicantComponent } from '../create-applicant/create-applicant.component';
import { StartCvParsingModalComponent }
  from '../start-cv-parsing-modal/start-cv-parsing-modal.component';

@Component({
  selector: 'app-applicants-head',
  templateUrl: './applicants-head.component.html',
  styleUrls: ['./applicants-head.component.scss'],
})
export class ApplicantsHeadComponent implements OnInit{
  public searchValue = '';
  public page: string = 'all';
  public creationData?: CreateApplicant;

  private readonly applicantPageToken: string = 'applicantPageToken';

  @Output() public search = new EventEmitter<string>();
  @Output() public applicantCreated = new EventEmitter<Observable<Applicant>>();
  @Output() public togglePage = new EventEmitter<string>();

  constructor(
    private readonly dialog: MatDialog,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
  ) {}

  public ngOnInit(): void {
    this.route.queryParams.subscribe((query) => {
      if (query['data']) {
        const latin1 = atob(query['data']);
        const json = decodeURIComponent(escape(latin1));

        if (query['variants']) {
          const variants: ApplicantCreationVariants = JSON.parse(json);

          this.dialog.open(CreateApplicantFromVariantsComponent, {
            width: '600px',
            height: '95vh',
            data: variants,
          });
        } else {
          const creationData: CreateApplicant = JSON.parse(json);

          this.creationData = creationData;
          this.showApplicantsCreateDialog();
        }

        this.router.navigate(['/applicants'], {
          queryParams: {
            variants: null,
            data: null,
          },
          queryParamsHandling: 'merge',
        });
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
      width: '600px',
      height: '95vh',
      autoFocus: false,
      data: this.creationData,
    });

    this.creationData = undefined;
    this.applicantCreated.emit(dialogRef.afterClosed());
  }

  public showUploadCvDialog(): void {
    this.dialog.open(StartCvParsingModalComponent, {
      width: '600px',
    });
  }

  public toggleFollowedOrAll(page: string): void {
    this.page = page;
    this.togglePage.emit(page);
  }
}
