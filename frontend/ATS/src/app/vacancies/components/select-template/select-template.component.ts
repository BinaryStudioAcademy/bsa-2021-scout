import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Observable, Subject } from 'rxjs';
import { map, startWith, takeUntil } from 'rxjs/operators';
import { MailTemplateTable } from 'src/app/shared/models/mail-template/mail-template-table';
import { MailTemplateService } from 'src/app/shared/services/mail-template.service';

@Component({
  selector: 'app-select-template',
  templateUrl: './select-template.component.html',
  styleUrls: ['./select-template.component.scss'],
})
export class SelectTemplateComponent implements OnInit {
  private readonly unsubscribe$: Subject<void> = new Subject<void>();
  mailTemplates: MailTemplateTable[] = [];
  selectedTemplate: MailTemplateTable = {
    id: '',
    title: '',
    subject: '',
    html: '',
    visibilitySetting: '',
    userCreated: '',
    dateCreation: new Date(),
    attachmentsCount: 0,
    isFollowed: false,
  }
  saveButtonDisable: boolean = true;
  templateControl = new FormControl();

  filteredOptions!: Observable<MailTemplateTable[]>;

  constructor(private modal: MatDialogRef<SelectTemplateComponent>,
    private mailTemplateService: MailTemplateService) { }

  ngOnInit(): void {
    this.getMailTemplates();

    this.templateControl.valueChanges.subscribe((template) => {
      if (typeof template !== 'string') {
        this.selectedTemplate = template;
        this.saveButtonDisable = false;
      }
      else {
        this.saveButtonDisable = true;
      }
    });
  }

  public getMailTemplates() {
    this.mailTemplateService
      .getList()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.mailTemplates = resp.body!;

          this.mailTemplates.forEach((d) => {
            d.html = d.html?.replace(/<\/?[^>]+(>|$)/g, ' ');
          });

          this.filteredOptions = this.templateControl.valueChanges.pipe(
            startWith(''),
            map(value => typeof value === 'string' ? value : value.title),
            map(title => title ? this._filter(title) : this.mailTemplates.slice()),
          );
        },
      );
  }

  private _filter(value: string): MailTemplateTable[] {
    const filterValue = value?.toLowerCase();
    return this.mailTemplates.filter(option => option.title?.toLowerCase().includes(filterValue));
  }
  displayFn(mailTemplate: MailTemplateTable): string {
    return mailTemplate && mailTemplate.title ?
      `${mailTemplate.title}` : '';
  }

  onSave() {
    this.modal.close(this.selectedTemplate.id);
  }
}
