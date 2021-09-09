import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';

import { NotificationService } from 'src/app/shared/services/notification.service';
// eslint-disable-next-line
import { EMPTY, Subject } from 'rxjs';
import { finalize, mergeMap, takeUntil } from 'rxjs/operators';
import {
  FilterDescription,
  FilterType,
  TableFilterComponent,
} from 'src/app/shared/components/table-filter/table-filter.component';
import { IOption } from 'src/app/shared/components/multiselect/multiselect.component';
import { ConfirmationDialogComponent } 
  from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { ArchivedProject } from '../../models/archived-project';
import { ArchiveDataService } from '../../services/archive-data.service';
import { UnarchiveProjectDialogComponent } 
  from '../unarchive-project-dialog/unarchive-project-dialog.component';
import { AuthenticationService } from 'src/app/users/services/auth.service';
import { AuthUserEventService } from 'src/app/users/services/auth-user-event.service';
import { User } from 'src/app/users/models/user';

@Component({
  selector: 'app-project-archive-list',
  templateUrl: './project-archive.component.html',
  styleUrls: ['./project-archive.component.scss'],
})
export class ProjectArchiveComponent implements OnInit, AfterViewInit, OnDestroy {
  displayedColumns: string[] = [
    'position',
    'name',
    'description',
    'team-info',
    'creation-date',
    'expiration-date',
    'archived-by',
    'actions',
  ];

  archivedProjects: ArchivedProject[] = [];
  filteredData: ArchivedProject[] = [];
  dataSource: MatTableDataSource<ArchivedProject> =
  new MatTableDataSource<ArchivedProject>();;
  isHrLead: boolean = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild('filter') public filter!: TableFilterComponent;

  public loading: boolean = true;
  public filterDescription: FilterDescription = [];

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private dialogService: MatDialog,
    private notificationService: NotificationService,
    private archiveDataService: ArchiveDataService,
    private authService: AuthenticationService,
    private authUserEventService: AuthUserEventService,
  ) {

  }

  public ngOnInit() {
    this.getArchivedProjects();

    this.authService
      .getUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (user) => {
          this.checkIsHrLead(user);
        },
        () => {
          this.notificationService.showErrorMessage('Failed to check user role.');
        },
      );

    this.authUserEventService.userChangedEvent$
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((user) => this.checkIsHrLead(user));
  }

  public ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public getArchivedProjects() {
    this.loading = true;
    this.archiveDataService.getArchivedProjects()
      .pipe(
        takeUntil(this.unsubscribe$),
        finalize(() => this.loading = false),
      )
      .subscribe((resp) => {
        this.archivedProjects = resp;

        this.dataSource.data = this.archivedProjects;
        this.renewFilterDescription();
        this.directive.applyFilter$.emit();
      });
  }

  public renewFilterDescription(): void {
    const detectedArchivedByHrIds: string[] = [];

    const archivedByHrs: IOption[] = [];

    this.archivedProjects.forEach((project) => {
      if (!detectedArchivedByHrIds.includes(project.archivedProjectData.user.id!)) {
        archivedByHrs.push({
          id: project.archivedProjectData.user.id!,
          value: project.archivedProjectData.user.id!,
          label: project.archivedProjectData.user.firstName + 
          ' ' + project.archivedProjectData.user.lastName,
        });

        detectedArchivedByHrIds.push(project.archivedProjectData.user.id!);
      }
    });

    this.filterDescription = [
      {
        id: 'name',
        name: 'Name',
      },
      {
        id: 'description',
        name: 'Description',
      },
      {
        id: 'team-info',
        name: 'Team info',
      },
      {
        id: 'creation-date',
        name: 'Creation date',
        type: FilterType.Date,
      },
      {
        id: 'expiration-date',
        name: 'Expiration date',
        type: FilterType.Date,
      },
      {
        id: 'archived-by',
        name: 'Archived by',
        type: FilterType.Multiple,
        multipleSettings: {
          options: archivedByHrs,
          valueSelector: (project: ArchivedProject) => project.archivedProjectData.user.id!,
        },
      },
    ];
  }

  public setFiltered(data: ArchivedProject[]): void {
    this.filteredData = data;

    this.dataSource.data = this.filteredData;

    this.directive?.applyFilter$.emit();
    this.dataSource.paginator?.firstPage();
  }

  public applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  public showUnarchiveConfirmDialog(projectToUnarchive: ArchivedProject): void {
    this.dialogService
      .open(UnarchiveProjectDialogComponent, {
        width: '400px',
        autoFocus: false,
        panelClass: 'unarchive-project-dialog',
        data: projectToUnarchive,
      })
      .afterClosed()
      .subscribe(
        (isUnarchived) => {
          if (isUnarchived) {
            this.removeItemFromTableData(projectToUnarchive);
          }
        });
  }

  public showDeleteConfirmDialog(projectToDelete: ArchivedProject): void {
    this.dialogService.open(ConfirmationDialogComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data: {
        action: 'Delete',
        entityName: 'project',
        additionalMessage: 'All related data such as vacancies will be deleted.',
      },
    })
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap(response => {
          if (response) {
            this.loading = true;
            return this.archiveDataService.deleteProject(projectToDelete);
          }
          return EMPTY;
        }),
        finalize(() => this.loading = false),
      )
      .subscribe(
        (_) => {
          this.removeItemFromTableData(projectToDelete);
          this.notificationService.showSuccessMessage(
            `Project ${projectToDelete.name} is deleted!`,
          );
        },
        () => {
          this.notificationService.showErrorMessage(
            'Project deleting is failed!',
          );
        },
      );
  }

  private removeItemFromTableData(item: ArchivedProject) {
    let position = this.archivedProjects.findIndex(project => project.id === item.id);
    this.archivedProjects.splice(position, 1);
    
    position = this.filteredData.findIndex(project => project.id === item.id);
    this.filteredData.splice(position, 1);

    this.dataSource.data = this.filteredData;    
    this.renewFilterDescription();
    this.directive.applyFilter$.emit();
  }

  private checkIsHrLead(user: User | null): void {
    this.isHrLead = user?.roles?.find((role) => role.name === 'HrLead')
      ? true
      : false;
  }
}