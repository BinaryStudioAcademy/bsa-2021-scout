import {Component, ViewChild, OnInit, Inject} from '@angular/core';
import { MatTable } from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { ApplicantsPool } from 'src/app/shared/models/applicants-pool/applicants-pool';
import { Subject } from 'rxjs';
import { PoolService } from 'src/app/shared/services/poolService';
import { takeUntil} from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
import { Tag } from 'src/app/shared/models/tags/tag';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';

const DATA: Applicant[] = [];

@Component({
  selector: 'app-pool-details-modal',
  templateUrl: './pool-details-modal.component.html',
  styleUrls: ['./pool-details-modal.component.scss'],
})
export class PoolDetailsModalComponent implements OnInit {

  public id: string = '';
  public pool!: ApplicantsPool;

  constructor(    
    private poolService : PoolService,
    @Inject(MAT_DIALOG_DATA) public data: string,
    private notificationService : NotificationService,
  ) {}

  displayedColumns: string[] = [
    'position',
    'name',
    'tags',    
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
    this.loadData(this.data);
  }

  loadData(id:string) {
    this.loading = true;
    this.poolService
      .getPool(id)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.pool = resp;
          this.dataSource.data = resp.applicants;
          this.updatePaginator();
        },
        (error) => {          
          this.notificationService.showErrorMessage(error);        
        },
        () => this.loading = false,
      );
  }

  updatePaginator() {
    this.dataSource.paginator = this.paginator;
    this.directive?.applyFilter$.emit();
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

