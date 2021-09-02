import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { Component, Inject, OnDestroy } from '@angular/core';
import { applicantGroup } from '../../validators/applicant-validator';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UpdateApplicant } from 'src/app/shared/models/applicants/update-applicant';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { Tag } from 'src/app/shared/models/tags/tag';
import { FileType } from 'src/app/shared/enums/file-type.enum';

@Component({
  selector: 'app-update-applicant',
  templateUrl: 'update-applicant.component.html',
  styleUrls: ['update-applicant.component.scss', '../../common/common.scss'],
})
export class UpdateApplicantComponent implements OnDestroy {
  public validationGroup: FormGroup | undefined = undefined;
  public loading: boolean = false;

  public updatedApplicant: UpdateApplicant = {
    id: '',
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
    hasCv: false,
    cv: null,
  };
  public allowedCvFileType = FileType.Pdf;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

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
    this.updatedApplicant.email = applicant.email;
    this.updatedApplicant.phone = applicant.phone ?? '';
    this.updatedApplicant.linkedInUrl = applicant.linkedInUrl ?? '';
    this.updatedApplicant.experience = applicant.experience ?? 0;
    this.updatedApplicant.experienceDescription = applicant.experienceDescription;
    this.updatedApplicant.skills = applicant.skills;
    this.updatedApplicant.tags.id = applicant.tags.id;
    Object.assign<Tag[], Tag[]>(this.updatedApplicant.tags.tagDtos, applicant.tags.tagDtos);
    this.updatedApplicant.hasCv = applicant.hasCv;
  }

  public updateApplicant(): void {
    this.loading = true;

    this.applicantsService
      .updateApplicant(this.updatedApplicant)
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
            'Cannot update the applicant',
          );
        },
      );
  }

  public updateTags(tags: Tag[]): void {
    this.updatedApplicant.tags.tagDtos = tags;
  }

  public uploadApplicantCv(files: File[]): void {
    this.updatedApplicant.cv = files[0];
  }

  public ngOnDestroy(): void {
    this.validationGroup?.reset();

    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
