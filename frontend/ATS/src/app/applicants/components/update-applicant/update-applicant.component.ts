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

  public updatedApplicant!: UpdateApplicant;
  public allowedCvFileType = FileType.Pdf;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
  @Inject(MAT_DIALOG_DATA) applicant: Applicant,
    private readonly applicantsService: ApplicantsService,
    private readonly dialogRef: MatDialogRef<UpdateApplicantComponent>,
    private readonly notificationsService: NotificationService,
  ) {
    this.validationGroup = applicantGroup;
    this.updatedApplicant = {
      id: applicant.id,
      firstName: applicant.firstName,
      lastName: applicant.lastName,
      email: applicant.email,
      phone: applicant.phone ?? '',
      linkedInUrl: applicant.linkedInUrl ?? '',
      experience: applicant.experience ?? 0,
      creationDate: applicant.creationDate,
      experienceDescription: applicant.experienceDescription,
      skills: applicant.skills,
      tags: {
        id: applicant.tags.id,
        tagDtos: [],
        elasticType: 1,
      },
      hasCv: applicant.hasCv,
      hasPhoto: applicant.hasPhoto,
      cv: null,
      photo: null,
    };
    Object.assign<Tag[], Tag[]>(this.updatedApplicant.tags.tagDtos, applicant.tags.tagDtos);
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

  public uploadApplicantPhoto(files: File[]): void {
    this.updatedApplicant.photo = files[0];
  }

  public ngOnDestroy(): void {
    this.validationGroup?.reset();

    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
