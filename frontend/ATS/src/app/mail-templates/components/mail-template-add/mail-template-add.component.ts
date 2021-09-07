import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { MailTemplateCreate } from 'src/app/shared/models/mail-template/mail-template-create';
import { MailTemplateService } from 'src/app/shared/services/mail-template.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { takeUntil } from 'rxjs/operators';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
  selector: 'app-mail-template-add',
  templateUrl: './mail-template-add.component.html',
  styleUrls: ['./mail-template-add.component.scss'],
})
export class MailTemplateAddComponent implements OnDestroy {

  mailTemplate: MailTemplateCreate = {} as MailTemplateCreate;
  files: File[] = [];
  isShow: boolean = false;
  popupVisible: boolean = false;
  editorValue: string = '';

  
  toolbarButtonOptions: any = {
    text: 'Show markup',
    stylingMode: 'text',
    onClick: () => (this.popupVisible = true),
  };

  hideButtonOptions: any = {
    text: 'Show all',
    stylingMode: 'text',
    onClick: () => {
      this.isShow = !this.isShow;
    },
  };
  
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
      {class: 'arial', name: 'Arial'},
      {class: 'times-new-roman', name: 'Times New Roman'},
      {class: 'calibri', name: 'Calibri'},
      {class: 'comic-sans-ms', name: 'Comic Sans MS'},
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
    private dialogRef: MatDialogRef<MailTemplateAddComponent>) {
    this.dialogRef.disableClose = true;
    this.dialogRef.backdropClick().subscribe((_) => this.onFormClose());
  }

  
  mailTemplateCreateForm = new FormGroup({
    'slug': new FormControl(this.mailTemplate.slug,
      [Validators.required,
        Validators.minLength(3),
        Validators.maxLength(200)]),
    'subject': new FormControl(this.mailTemplate.subject,
      [Validators.required,
        Validators.minLength(3),
        Validators.maxLength(200)]),
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
    this.mailTemplate = this.mailTemplateCreateForm.value;
    this.mailTemplate.visibilitySetting = this.mailTemplateCreateForm
      .controls['visibilitySetting'].value ? 1 : 0;
    this.loading = true;
    this.mailTemplateService
      .createMailTempalte(this.mailTemplate, this.files)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        () => {
          this.loading = false;

          this.notificationService.showSuccessMessage(
            `Template ${this.mailTemplate.slug} created!`,
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
