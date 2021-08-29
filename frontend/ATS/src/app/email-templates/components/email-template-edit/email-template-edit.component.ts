import { Component, Inject, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { MailTemplateService } from 'src/app/shared/services/mail-template.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { takeUntil } from 'rxjs/operators';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { MailTemplateTable } from 'src/app/shared/models/mail-template/mail-template-table';
import { MailTemplate } from 'src/app/shared/models/mail-template/mail-template';
import { MailAttachment } from 'src/app/shared/models/mail-attachment/mail-attachment';

@Component({
  selector: 'app-email-template-edit',
  templateUrl: './email-template-edit.component.html',
  styleUrls: ['./email-template-edit.component.scss'],
})
export class EmailTemplateEditComponent implements OnDestroy {

  mailTemplateUpdate: MailTemplate = {} as MailTemplate;
  mailTemplate: MailTemplate = {} as MailTemplate;
  mailAttachments: MailAttachment[] = [];
  files: File[] = [];

  public loading: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  editorConfig: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: 'auto',
    minHeight: '0',
    maxHeight: 'auto',
    width: 'auto',
    minWidth: '0',
    translate: 'yes',
    enableToolbar: true,
    showToolbar: true,
    placeholder: 'Enter text here...',
    defaultParagraphSeparator: '',
    defaultFontName: '',
    defaultFontSize: '',
    fonts: [
      { class: 'arial', name: 'Arial' },
      { class: 'times-new-roman', name: 'Times New Roman' },
      { class: 'calibri', name: 'Calibri' },
      { class: 'comic-sans-ms', name: 'Comic Sans MS' },
    ],
    customClasses: [
      {
        name: 'quote',
        class: 'quote',
      },
      {
        name: 'redText',
        class: 'redText',
      },
      {
        name: 'titleText',
        class: 'titleText',
        tag: 'h1',
      },
    ],
    uploadUrl: 'v1/image',
    uploadWithCredentials: false,
    sanitize: true,
    toolbarPosition: 'top',
    toolbarHiddenButtons: [
      ['bold', 'italic'],
      ['fontSize'],
    ],
  };

  constructor(
    private mailTemplateService: MailTemplateService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<EmailTemplateEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { mailTemplate: MailTemplateTable },
  ) {
    this.mailTemplateService
      .getMailTempalte(this.data.mailTemplate.id)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.mailTemplate = resp;
          console.log(this.mailTemplate);

          this.mailTemplateCreateForm.controls['slug'].setValue(this.mailTemplate.slug);
          this.mailTemplateCreateForm.controls['subject'].setValue(this.mailTemplate.subject);
          this.mailTemplateCreateForm.controls['visibilitySetting']
            .setValue(this.mailTemplate.visibilitySetting);
          this.mailTemplateCreateForm.controls['html'].setValue(this.mailTemplate.html);
          
          this.loading = false;
        },
        (error) => {
          this.loading = false;
          this.notificationService.showErrorMessage(error.message);
        },
      );
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
    console.log(this.data.mailTemplate.id);
    this.mailTemplateUpdate = this.mailTemplateCreateForm.value;
    this.mailTemplateUpdate.id = this.data.mailTemplate.id;
    console.log(this.mailTemplate.mailAttachments);
    this.mailTemplateUpdate.mailAttachments = this.mailTemplate.mailAttachments;
    this.mailTemplateUpdate.visibilitySetting = this.mailTemplateCreateForm
      .controls['visibilitySetting'].value ? 1 : 0;
    this.loading = true;
    console.log(this.mailTemplateUpdate);
    this.mailTemplateService
      .updateMailTempalte(this.mailTemplateUpdate, this.files)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        () => {
          this.loading = false;

          this.notificationService.showSuccessMessage(
            `Template ${this.mailTemplate.slug} updated!`,
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
    this.files = [];
    files.forEach((file: File) => {
      this.files.push(file);
    });

  }
}
