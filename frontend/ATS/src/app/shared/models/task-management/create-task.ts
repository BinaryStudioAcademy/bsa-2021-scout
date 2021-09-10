import { Task } from './task';

export class CreateTask {
  name: string;
  applicantId : string;
  dueDate: Date;    
  isDone: boolean;
  note?: string;  
  UsersIds: string[];

  constructor(task: Task)
  {    
    this.name = task.name;
    this.applicantId = task.applicant.id;
    this.dueDate = task.dueDate;    
    this.isDone = task.isDone;
    this.note = task.note;  
    this.UsersIds = task.teamMembers ? task.teamMembers.map(x=>{return x.id;}) : [];
  }
}