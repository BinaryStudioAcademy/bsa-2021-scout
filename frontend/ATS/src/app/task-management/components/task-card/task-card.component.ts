import { Component, OnInit, Input } from '@angular/core';
import { widgetInfo } from '../../models/widget-info';
import { Task } from 'src/app/shared/models/task-management/task';
import moment, { Moment } from 'moment';

@Component({
  selector: 'app-task-card',
  templateUrl: './task-card.component.html',
  styleUrls: ['./task-card.component.scss'],
})
export class TaskCardComponent implements OnInit{
  @Input() public task!: Task;

  public createdWidget : widgetInfo = {} as widgetInfo;
  public deadlineWidget : widgetInfo = {} as widgetInfo;
  public doneWidget : widgetInfo = {} as widgetInfo;
  public doneDateWidget : widgetInfo = {} as widgetInfo;
  

  constructor() { }

  ngOnInit() {
    const options = { day: 'numeric', month: 'short' };

    this.createdWidget = {
      icon: 'hourglass_top', 
      lineOne: moment(this.task.createdDate).format('Do MMM hh:mm'), 
      lineTwo: 'Created task',
    };
    this.deadlineWidget = {
      icon: 'running_with_errors', 
      lineOne: moment(this.task.dueDate).format('Do MMM hh:mm'),
      lineTwo: 'Deadline',
    };
    this.doneWidget = {
      icon: 'check_circle_outline', 
      lineOne: 'Yes', 
      lineTwo: 'Is done',
    };
    this.doneDateWidget  = {
      icon: 'event_available', 
      lineOne: moment(this.task.doneDate).format('Do MMM hh:mm'),
      lineTwo: 'Done',
    };
  }

  getClass()
  {
    return this.task.isDone ? 'is-done':'';
  }

}
