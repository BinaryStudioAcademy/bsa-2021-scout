import {Component, ViewChild, OnInit, AfterViewInit} from '@angular/core';
import { MatTable } from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
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
import { Router } from '@angular/router';
import { PoolDetailsModalComponent } from '../pool-details-modal/pool-details-modal.component';



const DATA: ApplicantsPool[] = [];

@Component({
  selector: 'app-application-pool',
  templateUrl: './application-pool.component.html',
  styleUrls: ['./application-pool.component.scss'],
})

export class ApplicationPoolComponent implements OnInit , AfterViewInit{

  constructor(
    private readonly dialogService: MatDialog, 
    private poolService : PoolService,
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

  loading : boolean = false;
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
      .getPools()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.loading = false;          
          const dataWithTotal = resp.map(
            value => {
              return {
                ...value, count: value.applicants.length};                
            },            
          );
          this.dataSource.data = dataWithTotal;
          this.updatePaginator();
        },
        (error) => {
          this.loading = false; 
          this.notificationService.showErrorMessage(error);
        },
      );
  }

  

  createPool(pool : CreatePool) {
    this.loading = true;
    this.poolService
      .createPool(pool)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {          
          this.dataSource.data.push(resp);          
          this.table.renderRows();          
          this.updatePaginator();
          this.notificationService.showSuccessMessage('Pool successfully added');
        },
        (error) => {          
          this.notificationService.showSuccessMessage(`Create pool error: ${error}`);
        },
        () => { this.loading = false;},
      );
  }

  updatePool(pool : UpdatePool) {
    this.loading = true;
    this.poolService
      .updatePool(pool)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.updateRowData(resp.body!);
        },
        (error) => {
          this.notificationService.showSuccessMessage('Update pool error');
          console.log(error);
        },
        () => (this.loading = false),
      );
  }

  onCreate()
  {
    let createDialog = this.dialogService.open(CreateTalentpoolModalComponent, {
      width: '600px',
    });

    createDialog.afterClosed().subscribe(result => {      
    });
    
    const dialogSubmitSubscription = 
    createDialog.componentInstance.submitClicked.subscribe(result => {
      this.createPool(result);      
      dialogSubmitSubscription.unsubscribe();
    });
  }

  onDetails(id: string)
  {
    this.dialogService.open(PoolDetailsModalComponent, {
      width: '800px',
      data: id,
    });
  }

  editPool(pool : ApplicantsPool)
  {
    let editDialog = this.dialogService.open(EditAppPoolModalComponent, {
      width: '600px',
      data: pool,      
    });

    editDialog.afterClosed().subscribe(result => {      
    });
    
    const dialogSubmitSubscription = 
    editDialog.componentInstance.submitClicked.subscribe(result => {
      this.updatePool(result);
      dialogSubmitSubscription.unsubscribe();
    });
  }

  updateRowData(row: ApplicantsPool){
    let source = this.dataSource.data.find(x=> x.id == row.id);
    if(source)
    {
      source.applicants = row.applicants;
      source.name = row.name;
      source.description = row.description;
      source.dateCreated = row.dateCreated;
      source.createdBy = row.createdBy;
      source.count = row.applicants.length;
    }    
  }


}
