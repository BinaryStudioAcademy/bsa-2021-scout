import { Subject } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { Component, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { applicantGroup } from '../../validators/applicant-validator';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { CreateApplicant } from 'src/app/shared/models/applicant/create-applicant';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-create-applicant',
  templateUrl: 'create-applicant.component.html',
  styleUrls: [ 'create-applicant.component.scss' ],
})

export class CreateApplicantComponent implements OnDestroy {
  public validationGroup: FormGroup|undefined = undefined;
  public createdApplicant: CreateApplicant = {
    id: '',
    firstName: '',
    lastName: '',
    middleName: '',
    birthDate: new Date(),
    email: '',
    phone: '',
    skype: '',
    experience: 0,
  };

  private $unsubscribe = new Subject();

  constructor(
    private readonly httpClient: HttpClientService,
    private readonly dialogRef: MatDialogRef<CreateApplicantComponent>,
    private readonly notificationsService: NotificationService,
  ) {
    this.validationGroup = applicantGroup;
  }

  public createApplicant(): void {
    this.httpClient.postClearRequest<Applicant>('/applicants', this.createApplicant)
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
  }
}