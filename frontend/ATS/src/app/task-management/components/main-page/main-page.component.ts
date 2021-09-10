import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Task } from 'src/app/shared/models/task-management/task';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { MatDialog } from '@angular/material/dialog';
import { AllInOneComponent} from '../modal/all-in-one/all-in-one.component';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TaskService } from 'src/app/shared/services/taskService';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { UserDataService } from 'src/app/users/services/user-data.service';
import { ApplicantShort } from 'src/app/shared/models/task-management/applicant-short';
import { UserInfo } from 'src/app/shared/models/task-management/user-short';
import { CreateTask } from 'src/app/shared/models/task-management/create-task'; 
import { UpdateTask } from 'src/app/shared/models/task-management/update-task';
import {TaskCardComponent} from '../task-card/task-card.component';

interface filterObject {
  userFilter:boolean;
  isDoneFilter: boolean;
}

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss'],
})

export class MainPageComponent implements OnInit,AfterViewInit{
  
  public doneFilter: boolean = false;
  public isDone : boolean = false;
  public tasks : Task[] = [];
  public filter : filterObject = {userFilter:false, isDoneFilter:false} as filterObject;
  public isReviewPage : boolean = false;
  public filterUser : string = 'All';
  public currentUserId : string = '';
  public loading : boolean = false;
  public applicants : ApplicantShort [] = [];
  public users : UserInfo [] = [];
  
  private unsubscribe$ = new Subject<void>();

  public allTasks : Task[] =  [];

  @ViewChild(TaskCardComponent)
  private taskCard!: TaskCardComponent;

  constructor (
    private readonly dialogService: MatDialog,     
    private notificationService: NotificationService,
    private taskService: TaskService,
    private userService: UserDataService,
    private applicantService: ApplicantsService,
  ) { }


  ngOnInit() : void {
    this.decodeJwt();
    this.loadData();     
  }

  ngAfterViewInit() {
    setTimeout(() => 0);
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

          this.loadRelated();             
          
        },
        (error) => {
          this.loading = false;
          this.notificationService.showErrorMessage(error);
        },
      );
  }

  loadRelated() {
    this.loadApplicants();
    this.loadUsers();    
  }

  
  loadApplicants() {
    this.applicantService
      .getApplicants()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.applicants = resp.map(value => {
            return {
              ...value, image:''};
          });
        },        
        (error) => {
        },
      );
  }

  loadUsers() {
    this.userService
      .getUsers()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.users = resp.map(value => {
            return {
              id: value.id, 
              firstName: value.firstName, 
              lastName: value.lastName, 
              image:value.avatarUrl};
          }) as UserInfo[];          
        },        
        (error) => {
        },
      );
  }

  toggleDone(isDone: boolean) {
    this.isReviewPage = false;
    this.filter.isDoneFilter = isDone;
    this.filterData();
  }

  toggleReview() {
    this.isReviewPage = true;
    this.filterData();
  }

  filterData()
  {
    if(this.isReviewPage)
    {
      this.tasks = this.allTasks.filter(x=> !x.isReviewed && 
        (this.currentUserId == '' || this.filter.userFilter == false ||
          (this.filter.userFilter && x.createdBy.id == this.currentUserId)
        ));
    }
    else {
      this.tasks = this.allTasks.filter(x=> x.isReviewed &&
        x.isDone == this.filter.isDoneFilter && 
        (this.currentUserId == '' || this.filter.userFilter == false ||
          (this.filter.userFilter && x.createdBy.id == this.currentUserId)
        )); 
    }
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
      data: { task: { name : '', 
        applicant : {firstName:'',lastName:'',image:'',id:''},
        teamMembers : [],
        dueDate : new Date(),
      },
      users:this.users, applicants: this.applicants,
      },
    });

    createDialog.afterClosed().subscribe((result) => {});

    const dialogSubmitSubscription = 
    createDialog.componentInstance.submitClicked.subscribe(result => {
      this.saveTask(new CreateTask(result));
      dialogSubmitSubscription.unsubscribe();
    });
  }

  saveTask(task: CreateTask) {
    this.taskService
      .createTask(task)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.loading = false;
          this.allTasks.push(resp);
          this.taskCard.updateTask(resp); 
          this.filterData(); 
          this.notificationService.showSuccessMessage(`Task ${task.name} created`);
        },
        (error) => {
          this.loading = false;
          this.notificationService.showErrorMessage(error);
        },
      );
  }

  editTask(task: Task) {
    let editDialog = this.dialogService.open(AllInOneComponent, {
      width: '600px',
      data: {task:task, users:this.users, applicants: this.applicants,
      },
      
    });

    editDialog.afterClosed().subscribe((result) => {
      if(result)
      {
        task.isReviewed = true;
      }
      this.filterData();
    });

    const dialogSubmitSubscription =
      editDialog.componentInstance.submitClicked.subscribe((result) => {
        this.updateTask(new UpdateTask(result));
        dialogSubmitSubscription.unsubscribe();
      });
  }

  updateTask(task:UpdateTask) {
    this.taskService
      .updateTask(task)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.loading = false;          
          let resultTaskIndx = this.allTasks.findIndex(x=>x.id == resp.body?.id);                   
          this.allTasks.splice(resultTaskIndx,1);
          this.allTasks.push(resp.body!);
          this.filterData(); 
          this.notificationService.showSuccessMessage(`Task ${task.name} modified`);
        },
        (error) => {
          this.loading = false;
          this.notificationService.showErrorMessage(error);
        },
      );
  }

  RemoveTask(task:Task) {
    const indx= this.allTasks.indexOf(task);
    this.allTasks.splice(indx,1);
    this.filterData();
  }
}
