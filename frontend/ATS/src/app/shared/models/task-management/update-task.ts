import { CreateTask } from './create-task';
import { Task } from './task';

export class UpdateTask extends CreateTask{
  id: string;
  
  constructor(task: Task) {
    super(task);
    this.id=task.id;
  }
}