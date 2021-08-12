import {AfterViewInit, Component, ViewChild, Inject, OnInit} from '@angular/core';
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
import { switchMap, takeUntil } from 'rxjs/operators';
import { PoolService } from 'src/app/shared/services/poolService';
import { CreatePool } from 'src/app/shared/models/applicants-pool/create-pool';
import { UpdatePool } from 'src/app/shared/models/applicants-pool/update-pool';



const DATA: ApplicantsPool[] = [];
// = [
//{id:'11',position:1,name:'Good',createdBy:'Alex',dateCreated:new Date(),description:'good for us',
//     applicants : [ 
//       {id: '3',firstName : 'Sergey', lastName: 'Ingurskiy', email:'ivan@email.com'},
//       {id: '5',firstName : 'Alex', lastName: 'Sergeev', email:'ivan@email.com'},
//     ]},
//{id:'21',position: 2, name: 'Best', createdBy: 'John', dateCreated: new Date(), applicants : []},
//   {id:'22',position: 3, name: 'Bad', createdBy: 'Dan', dateCreated: new Date(), applicants : []},
//{id:'31',position:4,name:'Ready to work',createdBy:'Lesly',dateCreated: new Date(),applicants : [ 
//     {id: '3',firstName : 'Sergey', lastName: 'Ingurskiy', email:'ivan@email.com'},
//     {id: '4',firstName : 'Vladimir', lastName: 'Lenin', email:'ivan@email.com'},
//     {id: '5',firstName : 'Alex', lastName: 'Sergeev', email:'ivan@email.com'},
//     {id: '6',firstName : 'Ivan', lastName: 'Notch', email:'ivan@email.com'},    
//   ]},
//{id:'115',position:5,name:'1st class',createdBy:'Olaf', dateCreated: new Date(), applicants : []},
//   {id:'99',position: 6, name: 'Not Enouth expirience', createdBy: 'Alex', dateCreated: new Date()
//     , applicants : [ 
//       {id: '1',firstName : 'Ivan', lastName: 'Petrov', email:'ivan@email.com'},
//     ]},
// ];

@Component({
  selector: 'app-application-pool',
  templateUrl: './application-pool.component.html',
  styleUrls: ['./application-pool.component.scss'],
})

export class ApplicationPoolComponent implements OnInit {

  constructor(private readonly dialogService: MatDialog, private poolService : PoolService) {}

  displayedColumns: string[] = [
    'position',
    'name',
    'createdBy',
    'dateCreated',
    'applicantsCount',
    'description',
    'actions',
  ];

  loading : boolean = false;
  dataSource = new MatTableDataSource(DATA);
  private unsubscribe$ = new Subject<void>();
  
  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;
  @ViewChild(StylePaginatorDirective) directive : StylePaginatorDirective | undefined;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator!;
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
          this.dataSource.data = resp;
        },
        (error) => (this.loading = false),
      );
  }

  createPool(pool : CreatePool) {
    this.loading = true;
    this.poolService
      .createPool(pool)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.loading = false;
          this.dataSource.data.push(resp);
        },
        (error) => (this.loading = false),
      );
  }

  updatePool(pool : UpdatePool) {
    this.loading = true;
    this.poolService
      .updatePool(pool)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.loading = false;
          this.updateRowData(resp.body!);
        },
        (error) => (this.loading = false),
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
    }    
  }

}
