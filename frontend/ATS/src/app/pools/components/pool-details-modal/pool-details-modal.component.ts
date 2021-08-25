import {Component, ViewChild, OnInit, Inject, AfterViewInit} from '@angular/core';
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
import { ApplicantIsSelected } from 'src/app/shared/models/applicants/applicant-select';
import { Tag } from 'src/app/shared/models/tags/tag';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';
import { FormGroup, FormControl } from '@angular/forms';

const DATA: ApplicantIsSelected[] = [];

@Component({
  selector: 'app-pool-details-modal',
  templateUrl: './pool-details-modal.component.html',
  styleUrls: ['./pool-details-modal.component.scss',
    '../create-talentpool-modal/create-talentpool-modal.component.scss'],
})
export class PoolDetailsModalComponent implements OnInit, AfterViewInit {

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

  public poolForm: FormGroup = new FormGroup({
    'createdBy': new FormControl({value:'', disabled:true}),
    'dateCreated': new FormControl({value:'', disabled:true}),    
    'name': new FormControl({value:'', disabled:true}),
    'description': new FormControl({value:'', disabled:true}),
    'company': new FormControl({value:'', disabled:true}),
    'applicants': new FormControl({value:'', disabled:true}),
    'id': new FormControl({value:'', disabled:true}),
  });
  
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
          this.poolForm.setValue(this.pool);
          const applicantsMod = resp.applicants
            .map(
              value => {
                return {
                  ...value, isShowAllTags: false};                
              },            
            );
          this.dataSource.data = applicantsMod;
          this.updatePaginator();
        },
        (error) => {          
          this.notificationService.showErrorMessage(error);
          this.loading = false;
        },
        () => this.loading = false,
      );
  }

  updatePaginator() {
    this.dataSource.paginator = this.paginator;
    this.directive?.applyFilter$.emit();
  }
  public getFirstTags(applicant: ApplicantIsSelected): Tag[] {
    if(applicant.tags)
    {
      return applicant.tags.tagDtos.length > 4
        ? applicant.tags.tagDtos.slice(0, 3)
        : applicant.tags.tagDtos;
    }
    return [];
  }

  public toggleAllTags(): void {
    this.isShowAllTags = this.isShowAllTags ? false : true;
  }

  public toggleTags(applicant: ApplicantIsSelected): void {
    applicant.isShowAllTags = applicant.isShowAllTags ? false : true;
  }


}

