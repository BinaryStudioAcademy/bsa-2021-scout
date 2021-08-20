import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { map, takeUntil } from 'rxjs/operators';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
import { ViewableApplicant } from 'src/app/shared/models/applicant/viewable-applicant';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
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

  public isDotMenuVisible = false;
  
  constructor(
    private readonly dialog: MatDialog,
    private readonly notificationsService: NotificationService,
    private readonly applicantsService: ApplicantsService,
    private readonly route: ActivatedRoute,
  ) {}

  public toggleDotMenu(): void {
    this.isDotMenuVisible = !this.isDotMenuVisible;
  }

  public showApplicantUpdateDialog(): void {
    const dialogRef = this.dialog.open(UpdateApplicantComponent, {
      width: '480px',
      height: 'min-content',
      autoFocus: false,
      data: this.applicant,
    });

    dialogRef.afterClosed()
      .pipe(
        map((a: Applicant) => {
          let viewableApplicant = (a as unknown) as ViewableApplicant;
          viewableApplicant.isShowAllTags = false;
  
          return viewableApplicant;
        }),
      )
      .subscribe((result: ViewableApplicant) => {
        if (result) {
          this.applicant = result;
        
          this.notificationsService.showSuccessMessage(
            'An applicant was succesfully updated',
            'Success!',
          );
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

}
