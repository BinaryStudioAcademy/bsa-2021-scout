import { Component, OnInit, ViewChild } from '@angular/core';
import { Task } from 'src/app/shared/models/task-management/task';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { MatDialog } from '@angular/material/dialog';
import { AllInOneComponent} from '../modal/all-in-one/all-in-one.component';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TaskService } from 'src/app/shared/services/taskService';

interface filterObject {
  userFilter:boolean;
  isDoneFilter: boolean;
}

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss'],
})

export class MainPageComponent implements OnInit{
  public doneFilter: boolean = false;
  public isDone : boolean = false;
  public tasks : Task[] = [];
  public filter : filterObject = {userFilter:false, isDoneFilter:false} as filterObject;
  public filterUser : string = 'All';
  public currentUserId : string = '';
  public loading : boolean = false;
  
  private unsubscribe$ = new Subject<void>();

  public allTasks : Task[] =  [];

  constructor (
    private readonly dialogService: MatDialog,     
    private notificationService: NotificationService,
    private taskService: TaskService,
  ) { }


  ngOnInit() : void {
    console.log('tasks loaded');
    this.loadData();      
  }

  loadData() {
    this.loading = true;

    this.taskService
      .getTasks()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.loading = false;

          this.allTasks= resp;
          this.filterData(); 
          
        },
        (error) => {
          this.loading = false;
          this.notificationService.showErrorMessage(error);
        },
      );
  }


  toggleDone(isDone: boolean) {
    this.filter.isDoneFilter = isDone;
    this.filterData();
  }

  filterData()
  {
    this.tasks = this.allTasks.filter(x=> 
      x.isDone == this.filter.isDoneFilter && 
      (this.currentUserId == '' || this.filter.userFilter == false ||
        (this.filter.userFilter && x.createdBy.id == this.currentUserId)
      ));    
  }

  toggleUser(value:string) {
    //binding doesn't work correctly on boolean (
    this.filterUser = value == 'Me' ? 'Me': 'All';
    this.filter.userFilter = value == 'Me' ? true: false;

    if(this.currentUserId =='') 
    {
      if(this.decodeJwt())
      {
        this.filterData();
      }           
    }
    else this.filterData();
  }

  decodeJwt ()  {    
    let result = false;
    const token = localStorage.getItem('accessToken');
    if(token) {
      try {
        let data = JSON.parse(atob(token.split('.')[1]));
        this.currentUserId = data.id;
        result = true;
      }
      catch {
      }      
    }
    return result;
  }

  createTask() {
    let createDialog = this.dialogService.open(AllInOneComponent, {
      width: '600px',      
    });

    createDialog.afterClosed().subscribe((result) => {});

    // const dialogSubmitSubscription = 
    // createDialog.componentInstance.submitClicked.subscribe(result => {
    //   this.createPool(result);      
    //   dialogSubmitSubscription.unsubscribe();
    // });
  }

  editTask(task: Task) {
    let editDialog = this.dialogService.open(AllInOneComponent, {
      width: '600px',
      data: task,
    });

    editDialog.afterClosed().subscribe((result) => {this.filterData();});

    // const dialogSubmitSubscription =
    //   editDialog.componentInstance.submitClicked.subscribe((result) => {
    //     this.updatePool(result);
    //     dialogSubmitSubscription.unsubscribe();
    //   });
  }
}
