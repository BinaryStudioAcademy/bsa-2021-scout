import { Subject } from 'rxjs';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { Component, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { applicantGroup } from '../../validators/applicant-validator';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { CreateApplicant } from 'src/app/shared/models/applicants/create-applicant';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { Tag } from 'src/app/shared/models/tags/tag';
import { FileType } from 'src/app/shared/enums/file-type.enum';
import { ApplicantCreationVariants } from 'src/app/shared/models/applicants/creation-variants';

@Component({
  selector: 'app-create-applicant-from-variants',
  templateUrl: 'create-applicant-from-variants.component.html',
  styleUrls: ['create-applicant-from-variants.component.scss', '../../common/common.scss'],
})
export class CreateApplicantFromVariantsComponent implements OnDestroy {
  public validationGroup: FormGroup | undefined = undefined;
  public loading: boolean = false;
  public showAllTags: boolean = false;

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
    private readonly dialogRef: MatDialogRef<CreateApplicantFromVariantsComponent>,
    private readonly notificationsService: NotificationService,
    @Inject(MAT_DIALOG_DATA) public data: ApplicantCreationVariants,
  ) {
    this.validationGroup = applicantGroup;

    if (data.cv) {
      this.createdApplicant.cv = data.cv;
    }
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

  public set(controlName: string, value: any): void {
    const control: AbstractControl = this.validationGroup?.get(controlName)!;
    control.setValue(control.value + value);
  }

  public addTag(tag: string): void {
    this.updateTags([...this.createdApplicant.tags.tagDtos, {
      id: '',
      tagName: tag,
    }]);
  }

  public removeTag(tag: string): void {
    const newTags = [...this.createdApplicant.tags.tagDtos];
    const index = newTags.findIndex(t => t.tagName === tag);

    if (index < 0) {
      return;
    }

    newTags.splice(index, 1);
    this.createdApplicant.tags.tagDtos = [...newTags];
  }

  public includesTag(tag: string): boolean {
    return this.createdApplicant.tags.tagDtos.some(t => t.tagName === tag);
  }

  public toggleTag(tag: string): void {
    if (this.includesTag(tag)) {
      return this.removeTag(tag);
    }

    return this.addTag(tag);
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
