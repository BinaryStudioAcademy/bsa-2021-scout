import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { Component, Inject, OnDestroy } from '@angular/core';
import { applicantGroup } from '../../validators/applicant-validator';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { UpdateApplicant } from 'src/app/shared/models/applicant/update-applicant';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-update-applicant',
  templateUrl: 'update-applicant.component.html',
  styleUrls: [ 'update-applicant.component.scss' ],
})

export class UpdateApplicantComponent implements OnDestroy {
  public validationGroup: FormGroup|undefined = undefined;
  public updatedApplicant: UpdateApplicant = {
    id: '',
    firstName: '',
    lastName: '',
    middleName: '',
    birthDate: new Date(),
    email: '',
    phone: '',
    skype: '',
    experience: 0,
  }
    
  private $unsubscribe = new Subject();

  constructor(
  @Inject(MAT_DIALOG_DATA) applicant: Applicant,
    private readonly httpClient: HttpClientService,
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
    this.httpClient.putRequest<Applicant>('/applicants', this.updateApplicant)
      .pipe(
        takeUntil(this.$unsubscribe),
      )
      .subscribe((result: Applicant) => {
        this.dialogRef.close(result);
      },
      (error: Error) => {
        this.notificationsService.showErrorMessage(error.message, 'Cannot update the applicant');
      });
  }

  public ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }
}