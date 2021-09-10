import {
  AfterViewInit,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MailTemplateTable } from 'src/app/shared/models/mail-template/mail-template-table';
import { MailTemplateService } from 'src/app/shared/services/mail-template.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { MatTableDataSource } from '@angular/material/table';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { EntityType } from 'src/app/shared/enums/entity-type.enum';
import { DeleteConfirmComponent } from
  'src/app/shared/components/delete-confirm/delete-confirm.component';
import { MailTemplateAddComponent } from '../mail-template-add/mail-template-add.component';
import { MailTemplateEditComponent } from '../mail-template-edit/mail-template-edit.component';
import { FollowedService } from 'src/app/shared/services/followedService';
import { SelectTemplateEntitiesComponent } 
  from '../select-template-entities/select-template-entities.component';

@Component({
  selector: 'app-mail-templates-list',
  templateUrl: './mail-templates-list.component.html',
  styleUrls: ['./mail-templates-list.component.scss'],
})
export class MailTemplatesListComponent implements AfterViewInit, OnInit, OnDestroy {
  displayedColumns: string[] = [
    'position',
    'title',
    'subject',
    'text',
    'dateCreation',
    'userCreated',
    'attachmentsCount',
    'actions',
  ];

  mailTemplates: MailTemplateTable[] = [];
  dataSource: MatTableDataSource<MailTemplateTable>;
  private readonly unsubscribe$: Subject<void> = new Subject<void>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  private followedSet: Set<string> = new Set();
  isFollowedPage: boolean = false;
  public loading: boolean = false;

  constructor(
    private mailTemplateService: MailTemplateService,
    private notificationService: NotificationService,
    private dialog: MatDialog,
    private followService: FollowedService,
  ) {
    this.followService.getFollowed(EntityType.MailTemplate)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(data =>
        data.forEach(item => this.followedSet.add(item.entityId)),
      );
    this.dataSource = new MatTableDataSource<MailTemplateTable>();
  }

  ngOnInit() {
    this.getMailTemplates();
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
            d.isFollowed = this.followedSet.has(d.id);
          });
          this.dataSource.data = this.mailTemplates;
          this.directive.applyFilter$.emit();
        },
      );
  }

  public switchToFollowed() {
    this.isFollowedPage = true;
    this.dataSource.data = this.mailTemplates.filter(mailTemplate => mailTemplate.isFollowed);
    this.directive.applyFilter$.emit();
  }

  public switchToMyTemplates() {
    this.isFollowedPage = false;
    this.dataSource.data = this.mailTemplates
      .filter(mailTemplate => mailTemplate.visibilitySetting);
    this.directive.applyFilter$.emit();
  }
  public switchAwayToAll() {
    this.isFollowedPage = false;
    this.dataSource.data = this.mailTemplates;
    this.directive.applyFilter$.emit();
  }
  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  public onBookmark(data: MailTemplateTable, perfomToFollowCleanUp: boolean = false) {
    let mailTemplateIndex: number = this.dataSource
      .data
      .findIndex(mailTemplate => mailTemplate.id === data.id)!;
    data.isFollowed = !data.isFollowed;
    if (data.isFollowed) {
      this.followService.createFollowed(
        {
          entityId: data.id,
          entityType: EntityType.MailTemplate,
        },
      ).subscribe();
    } else {
      this.followService.deleteFollowed(
        EntityType.MailTemplate, data.id,
      ).subscribe();
    }
    this.dataSource.data[mailTemplateIndex] = data;
    if (perfomToFollowCleanUp) {
      this.dataSource.data = this.dataSource.data.filter(mailTemplate => mailTemplate.isFollowed);
    }
    this.directive.applyFilter$.emit();
  }
  public OnCreate(): void {
    const dialogRef = this.dialog.open(MailTemplateAddComponent);

    dialogRef.afterClosed().subscribe(() => this.getMailTemplates());
  }

  public OnEdit(mailTemplateToEdit: MailTemplateTable): void {
    const dialogRef = this.dialog.open(MailTemplateEditComponent, {
      data: {
        mailTemplate: mailTemplateToEdit,
      },
    });

    dialogRef.afterClosed().subscribe(() => this.getMailTemplates());
  }

  public showDeleteConfirmDialog(mailTemplateToDelete: MailTemplateTable): void {
    const dialogRef = this.dialog.open(DeleteConfirmComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data: {
        entityName: 'template',
      },
    });

    dialogRef.afterClosed()
      .subscribe((response: boolean) => {
        if (response) {
          this.mailTemplateService
            .deleteMailTempalte(mailTemplateToDelete)
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe(_ => {
              this.notificationService
                .showSuccessMessage(`Template ${mailTemplateToDelete.title} deleted!`);
              this.getMailTemplates();
            });
        }
      });
  }

  isVacancyRequired: boolean = false;
  isProjectRequired: boolean = false;
  isApplicantRequired: boolean = false;

  sendEmail(mailTemplate: MailTemplateTable){
    this.parse(mailTemplate.html);

    this.dialog.open(SelectTemplateEntitiesComponent, 
      { width: '400px',
        data:{
          id: mailTemplate.id,
          isVacancyRequired: this.isVacancyRequired,
          isProjectRequired: this.isProjectRequired,
          isApplicantRequired: this.isApplicantRequired,
        }});
  }

  parse(html: string){
    this.isVacancyRequired = false;
    this.isProjectRequired = false;
    this.isApplicantRequired = false;
    var regExp = /{([^}]+)}/g;
    let words = html.match(regExp);
    let vacancyAttributesArray = ['title', 'description', 'requirements', 'salaryFrom', 'salaryTo'];
    let projectAttributesArray = ['title', 'logo', 'description', 'teamInfo', 'websiteLink'];
    let applicantAttributesArray = ['firstName', 'lastName', 'birthDate', 'phone', 'skype', 
      'linkedInUrl', 'experience', 'experienceDescription','skills'];

    words?.forEach(word => {
      if(word.substr(1,7) == 'vacancy')
      {
        vacancyAttributesArray.forEach(vacancyAttribute => {
          if(vacancyAttribute == word.slice(9, -1)){
            this.isVacancyRequired = true;
          }
        });
      }
      else if(word.substr(1,7) == 'project')
      {
        projectAttributesArray.forEach(projectAttribute => {
          if(projectAttribute == word.slice(9, -1)){
            this.isProjectRequired = true;
          }
        });
      }
      else if(word.substr(1,9) == 'applicant')
      {
        applicantAttributesArray.forEach(applicantAttribute => {
          if(applicantAttribute == word.slice(11, -1)){
            this.isApplicantRequired = true;
          }
        });
      }
    });
  }
}
