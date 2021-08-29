import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { MailTemplateCreate } from 'src/app/shared/models/mail-template/mail-template-create';
import { MailTemplateService } from 'src/app/shared/services/mail-template.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-email-template-add',
  templateUrl: './email-template-add.component.html',
  styleUrls: ['./email-template-add.component.scss'],
})
export class EmailTemplateAddComponent implements OnDestroy {

  mailTemplate: MailTemplateCreate = {} as MailTemplateCreate;
  files: File[] = [];
  
  public loading: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private mailTemplateService: MailTemplateService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<EmailTemplateAddComponent>,
  ) {
    this.dialogRef.disableClose = true;
    this.dialogRef.backdropClick().subscribe((_) => this.onFormClose());
  }

  
  mailTemplateCreateForm = new FormGroup({
    'slug': new FormControl(this.mailTemplate.slug,
      [Validators.required,
        Validators.minLength(3),
        Validators.maxLength(15)]),
    'subject': new FormControl(this.mailTemplate.subject,
      [Validators.required,
        Validators.minLength(3),
        Validators.maxLength(15)]),
    'html': new FormControl(this.mailTemplate.html,
      [Validators.required,
        Validators.minLength(10)]),
    'visibilitySetting': new FormControl(this.mailTemplate.visibilitySetting),
  });

  public onFormClose() {
    if (this.mailTemplateCreateForm.dirty) {
      if (confirm('Make sure you are saved everything. Continue?')) {
        this.dialogRef.close();
      }
    } else {
      this.dialogRef.close();
    }
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public onSubmited() {
    console.log(this.mailTemplate);
    this.mailTemplate = this.mailTemplateCreateForm.value;
    this.mailTemplate.visibilitySetting = this.mailTemplateCreateForm
      .controls['visibilitySetting'].value ? 1 : 0;
    this.loading = true;
    console.log(this.mailTemplateCreateForm.value);
    this.mailTemplateService
      .createMailTempalte(this.mailTemplate, this.files)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        () => {
          this.loading = false;

          this.notificationService.showSuccessMessage(
            `Project ${this.mailTemplate.slug} created!`,
          );
        },
        (error) => {
          this.loading = false;
          this.notificationService.showErrorMessage(error.message);
        },
      );

    this.dialogRef.close();
  }

  public uploadAttachments(files: File[]): void {
    files.forEach((file: File) => {
      this.files.push(file);
    });
      
  }
}
