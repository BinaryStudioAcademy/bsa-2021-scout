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
import { takeUntil } from 'rxjs/operators';
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

const DATA: ApplicantsPool[] = [];

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
  ) {}

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

  loading: boolean = false;
  mainData: ApplicantsPool[] = [];
  filteredData: ApplicantsPool[] = [];
  dataSource = new MatTableDataSource(DATA);
  private unsubscribe$ = new Subject<void>();

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

    this.poolService
      .getPools()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.loading = false;

          const dataWithTotal = resp.map((value) => {
            return {
              ...value,
              count: value.applicants.length,
            };
          });

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
    this.updatePaginator();
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
