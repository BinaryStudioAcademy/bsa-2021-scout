import {Component, ViewChild, OnInit, Input, AfterViewInit} from '@angular/core';
import { MatTable } from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { MatDialog } from '@angular/material/dialog';
import { ApplicantsPool } from 'src/app/shared/models/applicants-pool/applicants-pool';
import { Subject } from 'rxjs';
import { PoolService } from 'src/app/shared/services/poolService';
import { takeUntil} from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { ActivatedRoute } from '@angular/router';
import { Tag } from 'src/app/shared/models/tags/tag';

const DATA: Applicant[] = [];

@Component({
  selector: 'app-application-pool-details',
  templateUrl: './application-pool-details.component.html',
  styleUrls: ['./application-pool-details.component.scss'],
})
export class ApplicationPoolDetailsComponent implements AfterViewInit, OnInit {
  public id: string;
  public pool!: ApplicantsPool;

  constructor(
    private readonly dialogService: MatDialog, 
    private poolService : PoolService,
    private notificationService : NotificationService,
    private activateRoute :ActivatedRoute) {
    this.id = activateRoute.snapshot.params['id'];
  }

  displayedColumns: string[] = [
    'position',
    'name',
    'tags',
    //'actions',
  ];

  loading : boolean = false;
  isShowAllTags : boolean = false;
  dataSource = new MatTableDataSource(DATA);
  private unsubscribe$ = new Subject<void>();
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild(MatTable) table!: MatTable<ApplicantsPool>;

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.directive?.applyFilter$.emit();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if(this.dataSource.paginator) {
      this.directive?.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  ngOnInit() : void {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.poolService
      .getPool(this.id)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.loading = false;
          this.pool = resp;
          this.dataSource.data = resp.applicants;          
        },
        (error) => {
          this.loading = false; 
          this.notificationService.showErrorMessage(error);
        },
      );
  }

  public getFirstTags(applicant: Applicant): Tag[] {
    return applicant.tags.tagDtos.length > 6
      ? applicant.tags.tagDtos.slice(0, 5)
      : applicant.tags.tagDtos;
  }

  public toggleAllTags(): void {
    this.isShowAllTags = this.isShowAllTags ? false : true;
  }


}
