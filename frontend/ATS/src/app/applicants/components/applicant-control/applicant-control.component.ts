import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs/operators';
import { AddCandidateModalComponent } 
  from 'src/app/shared/components/modal-add-candidate/modal-add-candidate.component';
import { openFileFromUrl } from 'src/app/shared/helpers/openFileFromUrl';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
import { ViewableApplicant } from 'src/app/shared/models/applicant/viewable-applicant';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ApplicantDeleteConfirmComponent }
  from '../applicant-delete-confirm/applicant-delete-confirm.component';
import { UpdateApplicantComponent } from '../update-applicant/update-applicant.component';

@Component({
  selector: 'app-applicant-control',
  templateUrl: './applicant-control.component.html',
  styleUrls: ['./applicant-control.component.scss'],
})
export class ApplicantControlComponent {
  @Input() public applicant: ViewableApplicant|undefined = undefined;
  @Output() public deleteApplicantEvent = new EventEmitter<string>();
  @Output() public updateApplicantEvent = new EventEmitter<ViewableApplicant>();

  get isCvAvailable(): boolean {
    return this.applicant?.hasCv ?? false;
  }

  constructor(
    private readonly dialog: MatDialog,
    private readonly notificationsService: NotificationService,
    private readonly applicantsService: ApplicantsService,
    private readonly route: ActivatedRoute,
  ) {}

  public showApplicantUpdateDialog(): void {
    const dialogRef = this.dialog.open(UpdateApplicantComponent, {
      width: '480px',
      height: '95vh',
      autoFocus: false,
      data: this.applicant,
    });

    dialogRef.afterClosed()
      .pipe(
        map((a: Applicant) => {
          let viewableApplicant = (a as unknown) as ViewableApplicant;
          
          if (viewableApplicant) {
            viewableApplicant.isShowAllTags = false;
          }
  
          return viewableApplicant;
        }),
      )
      .subscribe((result: ViewableApplicant) => {
        if (result) {
          this.updateApplicantEvent.emit(result);
        }
      },
      (error: Error) => {
        this.notificationsService.showErrorMessage(
          error.message,
          'Cannot update the applicant',
        );
      });
  }

  public showDeleteConfirmDialog(): void {
    const dialogRef = this.dialog.open(ApplicantDeleteConfirmComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
    });

    dialogRef.afterClosed()
      .subscribe((response: boolean) => {
        if (response) {
          this.deleteApplicant();
        }
      });
  }

  public deleteApplicant(): void {
    this.applicantsService
      .deleteApplicant(this.applicant!.id)
      .subscribe(
        () => {
          this.deleteApplicantEvent.emit(this.applicant!.id);
        },
        (error: Error) => {
          this.notificationsService.showErrorMessage(
            error.message,
            'Cannot delete the applicant',
          );
        },
      );
  }

  public openVacancyAddModal(): void {
    this.dialog.open(AddCandidateModalComponent, {
      width: '400px',
      autoFocus: false,
      panelClass: 'applicants-options',
      data: {
        applicantId: this.applicant!.id,
      },
    });
  }

  public openCv(): void {
    this.applicantsService
      .getCv(this.applicant!.id)
      .subscribe(
        (cvFile) => {
          openFileFromUrl(cvFile.url);
        },
        (error: Error) => {
          this.notificationsService.showErrorMessage(
            error.message,
            'Cannot download cv',
          );
        },
      );
  }
}
