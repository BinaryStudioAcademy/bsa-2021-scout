import { Component, OnInit } from '@angular/core';
import { TaskService } from 'src/app/shared/services/taskService';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { Task } from 'src/app/shared/models/task-management/task';

@Component({
  selector: 'app-my-tasks',
  templateUrl: './my-tasks.component.html',
  styleUrls: ['./my-tasks.component.scss'],
})
export class MyTasksComponent implements OnInit {
  public tasks: Task[] = [];
  public loading : boolean = false;
  public isEnd: boolean = false;

  private unsubscribe$ = new Subject<void>();

  constructor(
    private taskService: TaskService,
    private notificationService: NotificationService,
  ) { }

  ngOnInit(): void {
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
          this.tasks= resp.filter(x=> !x.isDone);
          // && x.dueDate.getDate() == new Date().getDate() );
        },
        (error) => {
          this.loading = false;
          this.notificationService.showErrorMessage(error);
        },
      );
  }

}
