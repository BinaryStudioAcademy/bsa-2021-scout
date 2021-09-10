import { Component, OnInit, Input, Output,EventEmitter } from '@angular/core';
import { widgetInfo } from '../../models/widget-info';
import { Task } from 'src/app/shared/models/task-management/task';
import moment, { Moment } from 'moment';
import { MatDialog } from '@angular/material/dialog';
import {DeleteConfirmComponent} 
  from '../../../shared/components/delete-confirm/delete-confirm.component';
import { TaskService } from 'src/app/shared/services/taskService';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { TimezonePipe } from 'src/app/shared/pipes/timezone-pipe';


@Component({
  selector: 'app-task-card',
  templateUrl: './task-card.component.html',
  styleUrls: ['./task-card.component.scss'],
})
export class TaskCardComponent implements OnInit{
  @Input() public task!: Task;  
  @Input() public currentUserId!: string; 
  @Output() public deletedTask  = new EventEmitter<Task>();

  public createdWidget : widgetInfo = {} as widgetInfo;
  public deadlineWidget : widgetInfo = {} as widgetInfo;
  public doneWidget : widgetInfo = {} as widgetInfo;
  public doneDateWidget : widgetInfo = {} as widgetInfo;

  private unsubscribe$ = new Subject<void>();
  

  constructor(
    private readonly dialogService: MatDialog,
    private readonly taskService: TaskService,
    private readonly notificationService: NotificationService,
  ) { }

  ngOnInit() {
    this.updateTask(this.task);
    console.log(this.task);
  }

  getClass()
  {
    return this.task.isDone ? 'is-done':'';
  }

  isHot() {
    let hot = false;
    if(this.task.dueDate) {
      const end = this.task.dueDate.valueOf();
      const now = new Date().valueOf();      
      hot = (end - now < 2*1000*86400) || ((end < now) && !this.task.isDone);      
    } return hot;

  }

  updateTask(task:Task) {
    const options = { day: 'numeric', month: 'short' };

    this.createdWidget = {
      icon: 'hourglass_top', 
      lineOne: moment(task.dateCreated).format('Do MMM'), 
      lineTwo: 'Created task',
    };
    this.deadlineWidget = {
      icon: 'running_with_errors', 
      lineOne: task.dueDate ? new TimezonePipe()
        .transform(task.dueDate,'Do MMM'):'',
      // lineOne: moment(task.dueDate).local()
      //   .format('Do MMM'),
      lineTwo: 'Deadline',
    };
    this.doneWidget = {
      icon: 'check_circle_outline', 
      lineOne: 'Yes', 
      lineTwo: 'Is done',
    };
    this.doneDateWidget  = {
      icon: 'event_available', 
      lineOne: moment(task.doneDate).format('Do MMM'),      
      lineTwo: 'Done',
    };
    this.isHot();
    //console.log(task.doneDate);
  }

  public showDeleteConfirmDialog(task: Task): void {
    const dialogRef = this.dialogService.open(DeleteConfirmComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data: {
        entityName: 'Task',
      },
    });

    dialogRef.afterClosed().subscribe((response: boolean) => {
      if (response) {
        this.taskService
          .deleteTask(task.id)
          .pipe(takeUntil(this.unsubscribe$))
          .subscribe((_) => {
            this.deletedTask.emit(task);
            this.notificationService.showSuccessMessage(
              `Task ${task.name} deleted!`,
            );
          });
      }
    });


  }
}
