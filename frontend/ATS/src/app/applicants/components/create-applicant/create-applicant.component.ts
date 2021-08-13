import { Subject } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { Component, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { applicantGroup } from '../../validators/applicant-validator';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
import { CreateApplicant } from 'src/app/shared/models/applicant/create-applicant';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';

@Component({
  selector: 'app-create-applicant',
  templateUrl: 'create-applicant.component.html',
  styleUrls: [
    'create-applicant.component.scss',
    '../../common/common.scss',  
  ],
})

export class CreateApplicantComponent implements OnDestroy {
  public validationGroup: FormGroup|undefined = undefined;
  public createdApplicant: CreateApplicant = {
    firstName: '',
    lastName: '',
    middleName: '',
    email: '',
    phone: '',
    skype: '',
    experience: 0,
  };

  private $unsubscribe = new Subject();

  constructor(
    private readonly applicantsService: ApplicantsService,
    private readonly dialogRef: MatDialogRef<CreateApplicantComponent>,
    private readonly notificationsService: NotificationService,
  ) {
    this.validationGroup = applicantGroup;
  }

  public createApplicant(): void {
    this.applicantsService.addApplicant(this.createdApplicant)
      .pipe(
        takeUntil(this.$unsubscribe),
      )
      .subscribe((result: Applicant) => {
        this.dialogRef.close(result);
      },
      (error: Error) => {
        this.notificationsService.showErrorMessage(error.message, 'Cannot create an applicant');
      });
  }

  public ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();

    this.validationGroup?.reset();
  }
}