import { Subject } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { applicantGroup } from '../../validators/applicant-validator';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { CreateApplicant } from 'src/app/shared/models/applicants/create-applicant';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { Tag } from 'src/app/shared/models/tags/tag';
import { FileType } from 'src/app/shared/enums/file-type.enum';

@Component({
  selector: 'app-create-applicant',
  templateUrl: 'create-applicant.component.html',
  styleUrls: ['create-applicant.component.scss', '../../common/common.scss'],
})
export class CreateApplicantComponent implements OnInit, OnDestroy {
  public validationGroup: FormGroup | undefined = undefined;
  public loading: boolean = false;

  public createdApplicant: CreateApplicant = {
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    linkedInUrl: '',
    experience: 0,
    experienceDescription: '',
    skills: '',
    tags: {
      id: '',
      elasticType: 1,
      tagDtos: [],
    },
    cv: null,
    photo: null,
  };

  public allowedCvFileType = FileType.Pdf;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly applicantsService: ApplicantsService,
    private readonly dialogRef: MatDialogRef<CreateApplicantComponent>,
    private readonly notificationsService: NotificationService,
    @Inject(MAT_DIALOG_DATA) public data?: CreateApplicant,
  ) {
    this.validationGroup = applicantGroup;
  }

  public createApplicant(): void {
    this.loading = true;

    this.applicantsService
      .addApplicant(this.createdApplicant)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (result: Applicant) => {
          this.loading = false;
          this.dialogRef.close(result);
        },
        (error: Error) => {
          this.loading = false;

          this.notificationsService.showErrorMessage(
            error.message,
            'Cannot create an applicant',
          );
        },
      );
  }

  public ngOnInit(): void {
    if (this.data) {
      this.createdApplicant = {
        ...this.createdApplicant,
        ...this.data,
      };
    }
  }

  public updateTags(tags: Tag[]): void {
    this.createdApplicant.tags.tagDtos = tags;
  }

  public uploadApplicantCv(files: File[]): void {
    this.createdApplicant.cv = files[0];
  }

  public uploadApplicantPhoto(files: File[]): void {
    this.createdApplicant.photo = files[0];
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.validationGroup?.reset();
  }
}
