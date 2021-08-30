import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Task } from '../models/task-management/task';
import { CreateTask } from '../models/task-management/create-task';
import { UpdateTask } from '../models/task-management/update-task';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  public constructor(private readonly http: HttpClientService) {}
  public routePrefix = '/task';

  public getTasks() {
    return this.http.getRequest<Task[]>(`${this.routePrefix}`);
  }

  public getPool(id: string) {
    return this.http.getRequest<Task>(
      `${this.routePrefix}/${id}`);
  }

  public createPool(task : CreateTask) {
    return this.http.postRequest<Task>(
      `${this.routePrefix}`, task);
  }

  public updatePool(task: UpdateTask) {
    return this.http.putFullRequest<Task>(
      `${this.routePrefix}`,
      task,
    );
  }
  
  public deletePool(id: string) {
    return this.http.deleteFullRequest<Task>(
      `${this.routePrefix}/${id}`);
  }
}
