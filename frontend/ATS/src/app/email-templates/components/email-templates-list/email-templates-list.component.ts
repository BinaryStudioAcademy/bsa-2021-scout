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
import { EmailTemplateAddComponent } from '../email-template-add/email-template-add.component';
import { EmailTemplateEditComponent } from '../email-template-edit/email-template-edit.component';
import { FollowedService } from 'src/app/shared/services/followedService';

@Component({
  selector: 'app-email-templates-list',
  templateUrl: './email-templates-list.component.html',
  styleUrls: ['./email-templates-list.component.scss'],
})
export class EmailTemplatesListComponent implements AfterViewInit, OnInit, OnDestroy{
  displayedColumns: string[] = [
    'title',
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
      .subscribe(data=>
        data.forEach(item=>this.followedSet.add(item.entityId)),
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
          this.dataSource.data = this.mailTemplates;
          this.directive.applyFilter$.emit();
        },
      );
  }

  public switchToFollowed(){
    this.isFollowedPage = true;
    this.dataSource.data = this.dataSource.data.filter(mailTemplate=>mailTemplate.isFollowed);
    this.directive.applyFilter$.emit();
  }
  public switchAwayToAll(){
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

  public onBookmark(data: MailTemplateTable, perfomToFollowCleanUp: boolean = false){
    let projetIndex:number = this.dataSource.data.findIndex(project=>project.id === data.id)!;
    data.isFollowed = !data.isFollowed;
    if(data.isFollowed)
    {
      this.followService.createFollowed(
        {
          entityId: data.id,
          entityType: EntityType.MailTemplate,
        },
      ).subscribe();
    }else
    {
      this.followService.deleteFollowed(
        EntityType.MailTemplate, data.id,
      ).subscribe();
    }
    this.dataSource.data[projetIndex] = data;
    if(perfomToFollowCleanUp){
      this.dataSource.data = this.dataSource.data.filter(project=>project.isFollowed);
    }
    this.directive.applyFilter$.emit();
  }
  public OnCreate(): void {
    this.dialog.open(EmailTemplateAddComponent);

    this.dialog.afterAllClosed.subscribe((_) => this.getMailTemplates());
  }

  public OnEdit(mailTemplateToEdit: MailTemplateTable): void {
    this.dialog.open(EmailTemplateEditComponent, {
      data: {
        mailTemplate: mailTemplateToEdit,
      },
    });

    this.dialog.afterAllClosed.subscribe((_) => this.getMailTemplates());
  }

  public showDeleteConfirmDialog(mailTemplateToDelete: MailTemplateTable): void {
    const dialogRef = this.dialog.open(DeleteConfirmComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data:{
        entityName: 'Project',
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
}
