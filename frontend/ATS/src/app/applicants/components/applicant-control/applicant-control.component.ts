import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
} from '@angular/core';

import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { DeleteConfirmComponent } 
  from 'src/app/shared/components/delete-confirm/delete-confirm.component';
import { Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { AddCandidateModalComponent } 
  from 'src/app/shared/components/modal-add-candidate/modal-add-candidate.component';
import { openFileFromUrl } from 'src/app/shared/helpers/openFileFromUrl';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { ViewableApplicant } from 'src/app/shared/models/applicants/viewable-applicant';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { UpdateApplicantComponent } from '../update-applicant/update-applicant.component';
import { VacancyWithRecentActivity }
  from 'src/app/shared/models/candidate-to-stages/vacancy-with-recent-activity';
import { ApplicantHistoryComponent } from '../applicant-history/applicant-history.component';

@Component({
  selector: 'app-applicant-control',
  templateUrl: './applicant-control.component.html',
  styleUrls: ['./applicant-control.component.scss'],
})
export class ApplicantControlComponent implements OnDestroy {
  @Input() public applicant: ViewableApplicant | undefined = undefined;
  @Output() public deleteApplicantEvent = new EventEmitter<string>();
  @Output() public updateApplicantEvent = new EventEmitter<ViewableApplicant>();
  @Output() public reloadApplicantsEvent = new EventEmitter();
  @Output() public markAsFollowed = new EventEmitter<string>();

  public loading: boolean = false;
  public history: VacancyWithRecentActivity[] = [];
  public historyLoaded: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  get isCvAvailable(): boolean {
    return this.applicant?.hasCv ?? false;
  }

  constructor(
    private readonly dialog: MatDialog,
    private readonly notificationsService: NotificationService,
    private readonly applicantsService: ApplicantsService,
    private readonly route: ActivatedRoute,
  ) {}

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public showApplicantUpdateDialog(): void {
    const dialogRef = this.dialog.open(UpdateApplicantComponent, {
      width: '600px',
      height: '95vh',
      autoFocus: false,
      data: this.applicant,
    });

    dialogRef
      .afterClosed()
      .pipe(
        map((a: Applicant) => {
          let viewableApplicant = a as unknown as ViewableApplicant;

          if (viewableApplicant) {
            viewableApplicant.isShowAllTags = false;
          }

          return viewableApplicant;
        }),
      )
      .subscribe(
        (result: ViewableApplicant) => {
          if (result) {
            this.updateApplicantEvent.emit(result);
          }
        },
        (error: Error) => {
          this.notificationsService.showErrorMessage(
            error.message,
            'Cannot update the applicant',
          );
        },
      );
  }

  public showDeleteConfirmDialog(): void {
    const dialogRef = this.dialog.open(DeleteConfirmComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data: {
        entityName: 'Applicant',
      },
    });

    dialogRef.afterClosed().subscribe((response: boolean) => {
      if (response) {
        this.deleteApplicant();
      }
    });
  }

  public deleteApplicant(): void {
    this.loading = true;

    this.applicantsService
      .deleteApplicant(this.applicant!.id)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        () => {
          this.loading = false;
          this.deleteApplicantEvent.emit(this.applicant!.id);
        },
        (error: Error) => {
          this.loading = false;

          this.notificationsService.showErrorMessage(
            error.message,
            'Cannot delete the applicant',
          );
        },
      );
  }

  public onBookmark(){
    this.markAsFollowed.emit(this.applicant!.id);
  }

  public openVacancyAddModal(): void {
    this.dialog.open(AddCandidateModalComponent, {
      width: '500px',
      autoFocus: false,
      panelClass: 'applicants-options',
      data: {
        applicantId: this.applicant!.id,
      },
    })
      .afterClosed()
      .subscribe(_ => this.reloadApplicantsEvent.emit());
  }

  public openCv(): void {
    this.loading = true;

    this.applicantsService
      .getCv(this.applicant!.id)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (cvFile) => {
          this.loading = false;
          openFileFromUrl(cvFile.url);
        },
        (error: Error) => {
          this.loading = false;

          this.notificationsService.showErrorMessage(
            error.message,
            'Cannot download cv',
          );
        },
      );
  }

  public openHistory(): void {
    const openDialog = (): void => {
      this.dialog.open(ApplicantHistoryComponent, {
        data: this.history,
      });
    };

    if (this.historyLoaded) {
      return openDialog();
    }

    this.loading = true;

    this.applicantsService
      .getRecentActivity(this.applicant!.id)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        data => {
          this.loading = false;
          this.history = data;
          this.historyLoaded = true;

          openDialog();
        },
        () => {
          this.loading = false;
          this.notificationsService.showErrorMessage('Failed to load history');
        },
      );
  }
}
