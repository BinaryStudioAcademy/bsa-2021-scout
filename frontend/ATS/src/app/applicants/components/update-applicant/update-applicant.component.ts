import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { Component, Inject, OnDestroy } from '@angular/core';
import { applicantGroup } from '../../validators/applicant-validator';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UpdateApplicant } from 'src/app/shared/models/applicant/update-applicant';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';

@Component({
  selector: 'app-update-applicant',
  templateUrl: 'update-applicant.component.html',
  styleUrls: ['update-applicant.component.scss', '../../common/common.scss'],
})
export class UpdateApplicantComponent implements OnDestroy {
  public validationGroup: FormGroup | undefined = undefined;
  public updatedApplicant: UpdateApplicant = {
    id: '',
    firstName: '',
    lastName: '',
    middleName: '',
    email: '',
    phone: '',
    skype: '',
    linkedInUrl: '',
    experience: 0,
  };

  private $unsubscribe = new Subject();

  constructor(
  @Inject(MAT_DIALOG_DATA) applicant: Applicant,
    private readonly applicantsService: ApplicantsService,
    private readonly dialogRef: MatDialogRef<UpdateApplicantComponent>,
    private readonly notificationsService: NotificationService,
  ) {
    this.validationGroup = applicantGroup;

    this.updatedApplicant.id = applicant.id;
    this.updatedApplicant.firstName = applicant.firstName;
    this.updatedApplicant.lastName = applicant.lastName;
    this.updatedApplicant.middleName = applicant.middleName;
    this.updatedApplicant.email = applicant.email;
    this.updatedApplicant.phone = applicant.phone;
    this.updatedApplicant.skype = applicant.skype;
    this.updatedApplicant.experience = applicant.experience;
  }

  public updateApplicant(): void {
    this.applicantsService
      .updateApplicant(this.updatedApplicant)
      .pipe(takeUntil(this.$unsubscribe))
      .subscribe(
        (result: Applicant) => {
          this.dialogRef.close(result);
        },
        (error: Error) => {
          this.notificationsService.showErrorMessage(
            error.message,
            'Cannot update the applicant',
          );
        },
      );
  }

  public ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();

    this.validationGroup?.reset();
  }
}
