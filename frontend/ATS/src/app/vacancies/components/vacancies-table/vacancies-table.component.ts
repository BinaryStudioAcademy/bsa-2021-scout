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
import { VacancyStatus } from 'src/app/shared/models/vacancy/vacancy-status';
import { VacancyData } from 'src/app/shared/models/vacancy/vacancy-data';
import { VacancyDataService } from 'src/app/shared/services/vacancy-data.service';
import { Router } from '@angular/router';
import { VacancyCreate } from 'src/app/shared/models/vacancy/vacancy-create';
import { EditVacancyComponent } from '../edit-vacancy/edit-vacancy.component';
// eslint-disable-next-line
import { DeleteConfirmComponent } from 'src/app/shared/components/delete-confirm/delete-confirm.component';
import { EMPTY, Subject } from 'rxjs';
import { finalize, mergeMap, takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { FollowedService } from 'src/app/shared/services/followedService';
import { EntityType } from 'src/app/shared/enums/entity-type.enum';

import {
  FilterDescription,
  FilterType,
  PageDescription,
  TableFilterComponent,
} from 'src/app/shared/components/table-filter/table-filter.component';

import { IOption } from 'src/app/shared/components/multiselect/multiselect.component';
import { environment } from '../../../../environments/environment';
import { ArchivationService } from 'src/app/archive/services/archivation.service';
import { ConfirmationDialogComponent }
  from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';

const STATUES: VacancyStatus[] = [
  VacancyStatus.Active,
  VacancyStatus.Former,
  VacancyStatus.Invited,
  VacancyStatus.Vacation,
];

export interface IIndexable {
  [key: string]: any;
}

@Component({
  selector: 'app-vacancies-table',
  templateUrl: './vacancies-table.component.html',
  styleUrls: ['./vacancies-table.component.scss'],
})
export class VacanciesTableComponent
implements AfterViewInit, OnInit, OnDestroy
{
  displayedColumns: string[] = [
    'position',
    'title',
    'project',
    'teamInfo',
    'responsible',
    'creationDate',
    'candidatesAmount',
    'actions',
  ];

  public pageDescription: PageDescription = [
    {
      id: 'followed',
      selector: (vacancy: VacancyData) => vacancy.isFollowed,
    },
  ];

  dataSource: MatTableDataSource<VacancyData> =
  new MatTableDataSource<VacancyData>();

  mainData!: VacancyData[];
  filteredData!: VacancyData[];
  pageToken: string = 'followedVacancyPage';
  page?: string = localStorage.getItem(this.pageToken) ?? undefined;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild('input') serachField!: ElementRef;
  @ViewChild('filter') public filter!: TableFilterComponent;

  public filterDescription: FilterDescription = [];
  public loading: boolean = true;

  private followedSet: Set<string> = new Set();
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private service: VacancyDataService,
    private notificationService: NotificationService,
    private followService: FollowedService,
    private archivationService: ArchivationService,
  ) {}

  public ngOnInit(): void {
    this.followService
      .getFollowed(EntityType.Vacancy)
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap((data) => {
          data.forEach((item) => this.followedSet.add(item.entityId));
          return this.service.getList();
        }),
        finalize(() => (this.loading = false)),
      )
      .subscribe(
        (data) => {
          data.forEach((d) => {
            d.isFollowed = this.followedSet.has(d.id);
          });
          this.mainData = data;
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

  public ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'project':
          return item.project.name;
        case 'teamInfo':
          return item.project.teamInfo;
        case 'responsible':
          return (
            item.responsibleHr.firstName + ' ' + item.responsibleHr.lastName
          );
        default:
          return (item as IIndexable)[property];
      }
    };
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public getVacancies(): void {
    this.loading = true;
    this.service
      .getList()
      .pipe(
        takeUntil(this.unsubscribe$),
        finalize(() => (this.loading = false)),
      )
      .subscribe(
        (data) => {
          data.map((d, i) => ({
            ...d,
            position: i + 1,
            isFollowed: this.followedSet.has(d.id),
          }));

          this.mainData = data;
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

  public setPage(page?: string): void {
    this.filter.setPage(page);
    this.page = page;
  }

  public renewFilterDescription(): void {
    const detectedProjectIds: string[] = [];
    const detectedHrIds: string[] = [];

    const projects: IOption[] = [];
    const hrs: IOption[] = [];

    this.mainData.forEach((vac) => {
      if (!detectedProjectIds.includes(vac.project.id)) {
        projects.push({
          id: vac.project.id,
          value: vac.project.id,
          label: vac.project.name,
        });

        detectedProjectIds.push(vac.project.id);
      }

      if (!detectedHrIds.includes(vac.responsibleHr.id!)) {
        hrs.push({
          id: vac.responsibleHr.id!,
          value: vac.responsibleHr.id!,
          label: vac.responsibleHr.firstName + ' ' + vac.responsibleHr.lastName,
        });

        detectedHrIds.push(vac.responsibleHr.id!);
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
          sort: true,
          valueSelector: (vac: VacancyData) => vac.project.id,
        },
      },
      {
        id: 'teamInfo',
        property: 'project.teamInfo',
        name: 'Team info',
      },
      {
        id: 'responsibleHr',
        name: 'Responsible HR',
        type: FilterType.Multiple,
        multipleSettings: {
          options: hrs,
          sort: true,
          valueSelector: (vac: VacancyData) => vac.responsibleHr.id!,
        },
      },
      {
        id: 'creationDate',
        name: 'Creation date',
        type: FilterType.Date,
      },
      {
        id: 'candidatesAmount',
        name: 'Candidates amount',
        type: FilterType.Number,
        numberSettings: {
          min: 0,
          integer: true,
        },
      },
    ];
  }

  public setFiltered(data: VacancyData[]): void {
    this.filteredData = data;
    this.dataSource.data = this.filteredData;
    this.directive.applyFilter$.emit();
    this.dataSource.paginator?.firstPage();
  }

  public openDialog(): void {
    const dialogRef = this.dialog.open(EditVacancyComponent, {
      width: '800px',
      height: 'auto',
      data: {},
    });

    dialogRef.afterClosed().subscribe(() => this.getVacancies());
  }

  public onBookmark(data: VacancyData, perfomToFollowCleanUp: string = 'false'){
    let vacancyIndex: number = this.dataSource.data.findIndex(
      (vacancy) => vacancy.id === data.id,
    )!;
    data.isFollowed = !data.isFollowed;
    if (data.isFollowed) {
      this.followService
        .createFollowed({
          entityId: data.id,
          entityType: EntityType.Vacancy,
        })
        .subscribe();
    } else {
      this.followService
        .deleteFollowed(EntityType.Vacancy, data.id)
        .subscribe();
    }
    this.dataSource.data[vacancyIndex] = data;
    if (perfomToFollowCleanUp == 'true') {
      this.dataSource.data = this.dataSource.data.filter(
        (vacancy) => vacancy.isFollowed,
      );
    }
    this.directive.applyFilter$.emit();
  }

  public onEdit(vacancyEdit: VacancyCreate): void {
    this.dialog.open(EditVacancyComponent, {
      width: '800px',
      data: {
        vacancyToEdit: vacancyEdit,
      },
    });
    this.dialog.afterAllClosed.subscribe(() => this.getVacancies());
  }

  public saveVacancy(changedVacancy: VacancyData) {
    this.dataSource.data.unshift(changedVacancy);
  }

  public getStatus(index: number): string {
    return VacancyStatus[index];
  }

  public toStagedReRoute(id: string) {
    this.router.navigateByUrl(`candidates/${id}`);
  }

  public clearSearchField() {
    this.serachField.nativeElement.value = '';
    this.dataSource.filter = '';
    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  generateLink(id: string){
    return environment.clientUrl + `/vacancy/apply/${id}`;
  }

  successMessage(message: string){
    this.notificationService.showSuccessMessage(message);
  }

  public showArchiveConfirmDialog(vacancyToArchive: VacancyData): void {
    this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data: {
        action: 'Archive',
        entityName: 'vacancy',
      },
    })
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap(response => {
          if (response) {
            this.loading = true;
            return this.archivationService.archiveVacancy(vacancyToArchive.id);
          }
          return EMPTY;
        }),
        finalize(() => this.loading = false),
      )
      .subscribe(
        (_) => {
          let position = this.mainData.findIndex(vacancy => vacancy.id === vacancyToArchive.id);
          this.mainData.splice(position, 1);

          position = this.filteredData.findIndex(vacancy => vacancy.id === vacancyToArchive.id);
          this.filteredData.splice(position, 1);

          this.dataSource.data = this.filteredData;    
          this.renewFilterDescription();
          this.directive.applyFilter$.emit();

          this.notificationService.showSuccessMessage(
            `Vacancy ${vacancyToArchive.title} arhived!`,
          );
        },
        () => {
          this.notificationService.showErrorMessage(
            'Vacancy arhivation is failed!',
          );
        },
      );
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }
}
