import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { MatTable } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { MatDialog } from '@angular/material/dialog';
// eslint-disable-next-line max-len
import { CreateTalentpoolModalComponent } from '../create-talentpool-modal/create-talentpool-modal.component';
import { EditAppPoolModalComponent } from '../edit-app-pool-modal/edit-app-pool-modal.component';
import { ApplicantsPool } from 'src/app/shared/models/applicants-pool/applicants-pool';
import { Subject } from 'rxjs';
import { mergeMap, takeUntil } from 'rxjs/operators';
import { PoolService } from 'src/app/shared/services/poolService';
import { CreatePool } from 'src/app/shared/models/applicants-pool/create-pool';
import { UpdatePool } from 'src/app/shared/models/applicants-pool/update-pool';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { PoolDetailsModalComponent } from '../pool-details-modal/pool-details-modal.component';
// eslint-disable-next-line
import { DeleteConfirmComponent } from 'src/app/shared/components/delete-confirm/delete-confirm.component';
import {
  FilterDescription,
  FilterType,
} from 'src/app/shared/components/table-filter/table-filter.component';
import { IOption } from 'src/app/shared/components/multiselect/multiselect.component';
import { FollowedService } from 'src/app/shared/services/followedService';
import { EntityType } from 'src/app/shared/enums/entity-type.enum';
import { ApplicantIsSelected } from 'src/app/shared/models/applicants/applicant-select';
import { AddCandidateModalComponent } 
  from 'src/app/shared/components/modal-add-candidate/modal-add-candidate.component';

@Component({
  selector: 'app-application-pool',
  templateUrl: './application-pool.component.html',
  styleUrls: ['./application-pool.component.scss'],
})
export class ApplicationPoolComponent implements OnInit, AfterViewInit {
  constructor(
    private readonly dialogService: MatDialog,
    private poolService: PoolService,
    private notificationService: NotificationService,
    private followService: FollowedService,
  ) { 
    this.isFollowedPage = localStorage.getItem(this.followedPageToken) ? 
      localStorage.getItem(this.followedPageToken)! : 'false';
  }

  displayedColumns: string[] = [
    'position',
    'name',
    'createdBy',
    'dateCreated',
    'count',
    'description',
    'actions',
  ];

  public filterDescription: FilterDescription = [];

  mainData: ApplicantsPool[] = [];
  filteredData: ApplicantsPool[] = [];
  loading: boolean = false;
  dataSource = new MatTableDataSource(this.mainData);
  private unsubscribe$ = new Subject<void>();
  isFollowedPage: string = 'false';
  private followedSet: Set<string> = new Set();
  private readonly followedPageToken: string = 'followedPoolPage';
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild(MatTable) table!: MatTable<ApplicantsPool>;

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.updatePaginator();
  }

  updatePaginator() {
    this.dataSource.paginator = this.paginator;
    this.directive?.applyFilter$.emit();
    this.dataSource.paginator.firstPage();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.directive?.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.followService
      .getFollowed(EntityType.Pool)
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap((data) => {
          data.forEach((item) => this.followedSet.add(item.entityId));
          return this.poolService.getPools();
        }),
      )
      .subscribe(
        (resp) => {
          this.loading = false;

          const dataWithTotal = resp.map((value) => {
            return {
              ...value,
              count: value.applicants.length,
            };
          });
          dataWithTotal.forEach((d) => {
            d.isFollowed = this.followedSet.has(d.id);
          });
          if(localStorage.getItem(this.followedPageToken) == 'true'){
            this.dataSource.data = dataWithTotal.filter(item=>this.followedSet.has(item.id));
          }
          else
          {
            this.dataSource.data = dataWithTotal;
          }
          this.mainData = dataWithTotal;
          this.renewFilterDescription();
          this.updatePaginator();
        },
        (error) => {
          this.loading = false;
          this.notificationService.showErrorMessage(error);
        },
      );
  }

  public renewFilterDescription(): void {
    const detectedUserNames: string[] = [];
    const users: IOption[] = [];

    this.mainData.forEach((pool) => {
      if (!detectedUserNames.includes(pool.createdBy)) {
        users.push({
          id: pool.createdBy,
          value: pool.createdBy,
          label: pool.createdBy,
        });

        detectedUserNames.push(pool.createdBy);
      }
    });

    this.filterDescription = [
      {
        id: 'name',
        name: 'Name',
      },
      {
        id: 'createdBy',
        name: 'Created by',
        type: FilterType.Multiple,
        multipleSettings: {
          options: users,
          valueSelector: (pool) => pool.createdBy,
        },
      },
      {
        id: 'dateCreated',
        name: 'Creation date',
        type: FilterType.Date,
      },
      {
        id: 'applicantsCount',
        property: 'applicants.length',
        name: 'Applicants count',
        type: FilterType.Number,
        numberSettings: {
          integer: true,
          min: 0,
        },
      },
      {
        id: 'description',
        name: 'Description',
      },
    ];
  }

  public setFiltered(data: ApplicantsPool[]): void {
    this.filteredData = data;
    this.dataSource.data = data;

    if (localStorage.getItem(this.followedPageToken) == 'true') {
      this.dataSource.data = this.filteredData.filter((item) =>
        this.followedSet.has(item.id),
      );
    } else {
      this.dataSource.data = this.filteredData;
    }

    this.updatePaginator();
  }

  public switchToFollowed(){
    this.isFollowedPage = 'true';
    this.dataSource.data = this.dataSource.data.filter(pool=>pool.isFollowed);
    this.followService.switchRefreshFollowedPageToken('true', this.followedPageToken);
    this.directive.applyFilter$.emit();
  }
  public switchAwayToAll(){
    this.isFollowedPage = 'false';
    this.dataSource.data = this.mainData;
    this.followService.switchRefreshFollowedPageToken('false', this.followedPageToken);
    this.directive.applyFilter$.emit();
  }
  public onBookmark(data: ApplicantsPool, perfomToFollowCleanUp: string = 'false'){
    let poolIndex: number = this.dataSource.data.findIndex(
      (pool) => pool.id === data.id,
    )!;
    data.isFollowed = !data.isFollowed;
    if (data.isFollowed) {
      this.followService
        .createFollowed({
          entityId: data.id,
          entityType: EntityType.Pool,
        })
        .subscribe();
    } else {
      this.followService.deleteFollowed(EntityType.Pool, data.id).subscribe();
    }
    this.dataSource.data[poolIndex] = data;
    if(perfomToFollowCleanUp == 'true'){
      this.dataSource.data = this.dataSource.data.filter(pool=>pool.isFollowed);
    }
    this.directive.applyFilter$.emit();
  }

  createPool(pool: CreatePool) {
    this.loading = true;
    this.poolService
      .createPool(pool)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.mainData.push(resp);
          this.renewFilterDescription();
          this.table.renderRows();
          this.updatePaginator();
          this.notificationService.showSuccessMessage(
            'Pool successfully added',
          );
        },
        (error) => {
          this.notificationService.showSuccessMessage(
            `Create pool error: ${error}`,
          );
        },
        () => {
          this.loading = false;
        },
      );
  }

  updatePool(pool: UpdatePool) {
    this.loading = true;
    this.poolService
      .updatePool(pool)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.loading = false;
          this.updateRowData(resp.body!);
        },
        (error) => {
          this.loading = false;
          this.notificationService.showSuccessMessage('Update pool error');
        },
      );
  }

  onCreate() {
    let createDialog = this.dialogService.open(CreateTalentpoolModalComponent, {
      width: '600px',
    });

    createDialog.afterClosed().subscribe((result) => {});

    const dialogSubmitSubscription =
      createDialog.componentInstance.submitClicked.subscribe((result) => {
        this.createPool(result);
        dialogSubmitSubscription.unsubscribe();
      });
  }

  onAddApplicantsFromPoolToVacancy(applicants: ApplicantIsSelected[]){
    if(applicants != null && applicants.length != 0){
      let ids: string[] = [];

      applicants.forEach(applicant => {
        ids.push(applicant.id);
      });

      this.dialogService.open(AddCandidateModalComponent, {
        width: '400px',
        autoFocus: false,
        panelClass: 'applicants-options',
        data: {
          applicantsIds: ids,
        },
      });
    }
    else
    {
      this.notificationService.showInfoMessage('There is no applicants in pool');
    }
  }

  onDetails(id: string) {
    this.dialogService.open(PoolDetailsModalComponent, {
      width: '800px',
      height: '90vh',
      data: id,
      panelClass: 'pool-dialog-container',
    });
  }

  editPool(pool: ApplicantsPool) {
    let editDialog = this.dialogService.open(EditAppPoolModalComponent, {
      width: '600px',
      data: pool,
    });

    editDialog.afterClosed().subscribe((result) => {});

    const dialogSubmitSubscription =
      editDialog.componentInstance.submitClicked.subscribe((result) => {
        this.updatePool(result);
        dialogSubmitSubscription.unsubscribe();
      });
  }

  markPool(row: ApplicantsPool) {}

  updateRowData(row: ApplicantsPool) {
    let source = this.mainData.find((x) => x.id == row.id);
    if (source) {
      source.applicants = row.applicants;
      source.name = row.name;
      source.description = row.description;
      source.dateCreated = row.dateCreated;
      source.createdBy = row.createdBy;
      source.count = row.applicants.length;
    }
  }

  public showDeleteConfirmDialog(pool: ApplicantsPool): void {
    const dialogRef = this.dialogService.open(DeleteConfirmComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data: {
        entityName: 'Pool',
      },
    });

    dialogRef.afterClosed().subscribe((response: boolean) => {
      if (response) {
        this.poolService
          .deletePool(pool.id)
          .pipe(takeUntil(this.unsubscribe$))
          .subscribe((_) => {
            const mainDataIndex = this.mainData.indexOf(pool);
            this.mainData.splice(mainDataIndex, 1);
            this.renewFilterDescription();
            this.table.renderRows();
            this.updatePaginator();
            this.notificationService.showSuccessMessage(
              `Pool ${pool.name} deleted!`,
            );
          });
      }
    });
  }
}
