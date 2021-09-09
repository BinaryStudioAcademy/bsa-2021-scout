import {
  AfterViewInit,
  Component,
  ViewChild,
  ElementRef,
  OnDestroy,
  OnInit,
} from '@angular/core';

import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';

import { EMPTY, Subject } from 'rxjs';
import { finalize, mergeMap, takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';

import {
  FilterDescription,
  FilterType,
} from 'src/app/shared/components/table-filter/table-filter.component';

import { IOption } from 'src/app/shared/components/multiselect/multiselect.component';
import { ArchivationService } from 'src/app/archive/services/archivation.service';
import { ConfirmationDialogComponent } 
  from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { ArchivedVacancy } from '../../models/archived-vacancy';
import { ArchiveDataService } from '../../services/archive-data.service';
import { AuthenticationService } from 'src/app/users/services/auth.service';
import { AuthUserEventService } from 'src/app/users/services/auth-user-event.service';
import { User } from 'src/app/users/models/user';

export interface IIndexable {
  [key: string]: any;
}

@Component({
  selector: 'app-vacancy-archive-table',
  templateUrl: './vacancy-archive.component.html',
  styleUrls: ['./vacancy-archive.component.scss'],
})
export class VacancyArchiveComponent
implements AfterViewInit, OnInit, OnDestroy
{
  displayedColumns: string[] = [
    'position',
    'title',
    'project',
    'created',
    'responsible-hr',
    'expiration-date',
    'archived-by',
    'status',
    'actions',
  ];

  dataSource: MatTableDataSource<ArchivedVacancy> =
  new MatTableDataSource<ArchivedVacancy>();

  archivedVacancies!: ArchivedVacancy[];
  filteredData!: ArchivedVacancy[];
  isHrLead: boolean = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild('input') serachField!: ElementRef;

  public filterDescription: FilterDescription = [];
  public loading: boolean = true;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private dialogService: MatDialog,
    private notificationService: NotificationService,
    private archiveDataService: ArchiveDataService,
    private archivationService: ArchivationService,
    private authService: AuthenticationService,
    private authUserEventService: AuthUserEventService,
  ) {}

  public ngOnInit(): void {
    this.getArchivedVacancies();

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

  public ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'title':
          return item.title;
        case 'project':
          return item.projectName;
        case 'created':
          return item.creationDate;
        case 'responsible-hr':
          return item.responsibleHr.firstName + ' ' + item.responsibleHr.lastName;
        case 'expiration-date':
          return item.archivedVacancyData.expirationDate;
        case 'archived-by':
          return item.archivedVacancyData.user.firstName + 
          ' ' + item.archivedVacancyData.user.lastName;
        default:
          return (item as IIndexable)[property];
      }
    };
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public getArchivedVacancies(): void {
    this.loading = true;
    this.archiveDataService.getArchivedVacancies()
      .pipe(
        takeUntil(this.unsubscribe$),
        finalize(() => this.loading = false),
      )
      .subscribe(
        (data) => {
          this.archivedVacancies = data;
          this.filteredData = data;       
          this.dataSource.data = data;
       

          this.renewFilterDescription();
          this.directive.applyFilter$.emit();
        },
        () => {
          this.loading = false;
          this.notificationService.showErrorMessage('Failed to get vacancies.');
        },
      );
  }

  public renewFilterDescription(): void {
    const detectedProjectIds: string[] = [];
    const detectedHrIds: string[] = [];
    const detectedArchivedByHrIds: string[] = [];

    const projects: IOption[] = [];
    const hrs: IOption[] = [];
    const archivedByHrs: IOption[] = [];

    this.archivedVacancies.forEach((vac) => {
      if (!detectedProjectIds.includes(vac.projectName)) {
        projects.push({
          id: vac.projectName,
          value: vac.projectName,
          label: vac.projectName,
        });

        detectedProjectIds.push(vac.projectName);
      }

      if (!detectedHrIds.includes(vac.responsibleHr.id!)) {
        hrs.push({
          id: vac.responsibleHr.id!,
          value: vac.responsibleHr.id!,
          label: vac.responsibleHr.firstName + ' ' + vac.responsibleHr.lastName,
        });

        detectedHrIds.push(vac.responsibleHr.id!);
      }

      if (!detectedArchivedByHrIds.includes(vac.archivedVacancyData.user.id!)) {
        archivedByHrs.push({
          id: vac.archivedVacancyData.user.id!,
          value: vac.archivedVacancyData.user.id!,
          label: vac.archivedVacancyData.user.firstName 
            + ' ' + vac.archivedVacancyData.user.lastName,
        });

        detectedArchivedByHrIds.push(vac.archivedVacancyData.user.id!);
      }
    });

    this.filterDescription = [
      {
        id: 'title',
        name: 'Title',
      },
      {
        id: 'project',
        name: 'Project',
        type: FilterType.Multiple,
        multipleSettings: {
          options: projects,
          valueSelector: (vac: ArchivedVacancy) => vac.projectName,
        },
      },
      {
        id: 'created',
        name: 'Creation date',
        type: FilterType.Date,
      },
      {
        id: 'expiration-date',
        name: 'Expiration date',
        type: FilterType.Date,
      },
      {
        id: 'responsible-hr',
        name: 'Responsible HR',
        type: FilterType.Multiple,
        multipleSettings: {
          options: hrs,
          valueSelector: (vac: ArchivedVacancy) => vac.responsibleHr.id!,
        },
      },
      {
        id: 'archived-by',
        name: 'Archived by',
        type: FilterType.Multiple,
        multipleSettings: {
          options: archivedByHrs,
          valueSelector: (vac: ArchivedVacancy) => vac.archivedVacancyData.user.id!,
        },
      },
    ];
  }

  public setFiltered(data: ArchivedVacancy[]): void {
    this.filteredData = data;

    this.dataSource.data = this.filteredData;

    this.directive.applyFilter$.emit();
    this.dataSource.paginator?.firstPage();
  }
  
  public clearSearchField() {
    this.serachField.nativeElement.value = '';
    this.dataSource.filter = '';
    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  public applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  public showUnarchiveConfirmDialog(vacancyToUnarchive: ArchivedVacancy): void {
    this.dialogService.open(ConfirmationDialogComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data: {
        action: 'Unarchive',
        entityName: 'vacancy',
      },
    })
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap(response => {
          if (response) {
            this.loading = true;
            return this.archivationService.unarchiveVacancy(vacancyToUnarchive);
          }
          return EMPTY;
        }),
        finalize(() => this.loading = false),
      )
      .subscribe(
        (_) => {
          this.removeItemFromTableData(vacancyToUnarchive);
          this.notificationService.showSuccessMessage(
            `Vacancy ${vacancyToUnarchive.title} is unarhived!`,
          );
        },
        () => {
          this.notificationService.showErrorMessage(
            'Vacancy unarhivation is failed!',
          );
        },
      );
  }

  public showDeleteConfirmDialog(vacancyToDelete: ArchivedVacancy): void {
    this.dialogService.open(ConfirmationDialogComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data: {
        action: 'Delete',
        entityName: 'vacancy',
        additionalMessage: 'All related data will be deleted.',
      },
    })
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap(response => {
          if (response) {
            this.loading = true;
            return this.archiveDataService.deleteVacancy(vacancyToDelete);
          }
          return EMPTY;
        }),
        finalize(() => this.loading = false),
      )
      .subscribe(
        (_) => {
          this.removeItemFromTableData(vacancyToDelete);
          this.notificationService.showSuccessMessage(
            `Vacancy ${vacancyToDelete.title} is deleted!`,
          );
        },
        () => {
          this.notificationService.showErrorMessage(
            'Vacancy deleting is failed!',
          );
        },
      );
  }

  private removeItemFromTableData(item: ArchivedVacancy) {
    let position = this.archivedVacancies.findIndex(vacancy => vacancy.id === item.id);
    this.archivedVacancies.splice(position, 1);
    
    position = this.filteredData.findIndex(vacancy => vacancy.id === item.id);
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
