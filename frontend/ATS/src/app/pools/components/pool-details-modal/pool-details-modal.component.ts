import {Component, ViewChild, OnInit, Inject, AfterViewInit, Output, EventEmitter} 
  from '@angular/core';
import { MatTable, MatTableDataSource} from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ApplicantsPool } from 'src/app/shared/models/applicants-pool/applicants-pool';
import { Subject } from 'rxjs';
import { PoolService } from 'src/app/shared/services/poolService';
import { takeUntil} from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ApplicantIsSelected } from 'src/app/shared/models/applicants/applicant-select';
import { Tag } from 'src/app/shared/models/tags/tag';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SelectModalComponent } from '../modal/select-modal/select-modal.component';
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
  public poolApplicants!: ApplicantIsSelected[];
  public allApplicants!: ApplicantIsSelected[];
  @Output() submitClicked = new EventEmitter<any>();

  @ViewChild(MatTable) table!: MatTable<ApplicantsPool>;

  constructor(
    private readonly dialogService: MatDialog,  
    private poolService : PoolService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<SelectModalComponent>,
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
    'name': new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(20)]),
    'description': new FormControl('', [
      Validators.required,
      Validators.minLength(10),
    ]),
    'company': new FormControl({value:'', disabled:true}),
    'applicants': new FormControl({value:'', disabled:true}),
    'id': new FormControl({value:'', disabled:true}),
  });
  
  
  @ViewChild(MatSort) sort!: MatSort;  

  ngAfterViewInit() {    
    this.dataSource.sort = this.sort;    
  }
  
  
  ngOnInit() : void {
    this.loadData(this.data.id);
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
          this.poolApplicants = resp.applicants
            .map(
              value => {
                return {
                  ...value, isShowAllTags: false};                
              },            
            );          
          this.dataSource.data = this.poolApplicants;          
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

  addApplicants() {
    let addDialog = this.dialogService.open(SelectModalComponent, {
      width: '600px',
      height: '360px',
      data: {selected:this.dataSource.data, applicants: this.data.applicants,
      },
        
    });
  
    addDialog.afterClosed().subscribe((result) => {});
  
    const dialogSubmitSubscription =
        addDialog.componentInstance.submitClicked.subscribe((result) => {
          this.dataSource.data = result.selected;
          this.table.renderRows();
          dialogSubmitSubscription.unsubscribe();
        });
  }
  

  compareById(o1:any, o2:any): boolean{    
    return o1.id === o2.id;
  }

  onUserChange(event:any) {
    // this.table.renderRows();    
  }

  getClass(control:string)
  {
    return this.poolForm.controls[control].dirty && this.poolForm.controls[control].errors?
      'invalid-input':'';
  }

  save() {
    const data = this.poolForm.value;
    data.id = this.data.id;
    data.applicantsIds = this.dataSource.data.map((x) => x.id).join();
    this.submitClicked.emit(data);
    this.dialogRef.close();

  }



}

