import {Component, ViewChild, OnInit, Inject, AfterViewInit} from '@angular/core';
import { MatTableDataSource} from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ApplicantsPool } from 'src/app/shared/models/applicants-pool/applicants-pool';
import { Subject } from 'rxjs';
import { PoolService } from 'src/app/shared/services/poolService';
import { takeUntil} from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ApplicantIsSelected } from 'src/app/shared/models/applicants/applicant-select';
import { Tag } from 'src/app/shared/models/tags/tag';
import { MAT_DIALOG_DATA} from '@angular/material/dialog';
import { FormGroup, FormControl } from '@angular/forms';
import { getApplicantAvatar } from 'src/app/shared/helpers/avatar';

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
    'firstName',    
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
  
  
  @ViewChild(MatSort) sort!: MatSort;  

  ngAfterViewInit() {    
    this.dataSource.sort = this.sort;    
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
        },
        (error) => {          
          this.notificationService.showErrorMessage(error);
          this.loading = false;
        },
        () => this.loading = false,
      );
  }

  public getAvatar(applicant: ApplicantIsSelected): string {
    return getApplicantAvatar(applicant);
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

