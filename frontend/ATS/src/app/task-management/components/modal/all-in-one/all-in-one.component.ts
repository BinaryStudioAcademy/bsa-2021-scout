import { Component, Output, EventEmitter,Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import { ApplicantShort } from 'src/app/shared/models/task-management/applicant-short';
import { Task } from 'src/app/shared/models/task-management/task';

@Component({
  selector: 'app-all-in-one',
  templateUrl: './all-in-one.component.html',
  styleUrls: ['./all-in-one.component.scss'],
})
export class AllInOneComponent {
  @Output() submitClicked = new EventEmitter<any>();
  public task: Task = {} as Task;
  

  public taskForm: FormGroup = new FormGroup({
    'name': new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(20)]),
    'isDone': new FormControl('', [
      Validators.required,
      Validators.minLength(10),
    ]),
    'candidate': new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(20)]),
    'dueDate': new FormControl(''),      
    'note': new FormControl(''),
  });
  public formType:string = '';

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Task,
    public dialogRef: MatDialogRef<AllInOneComponent>,
  ) {}

  ngOnInit() {
    if(this.data) {
      this.formType = 'Edit';
      this.task = this.data;
    }
    else {
      this.formType = 'Create';
      this.task.name = '';
      this.task.applicant = {firstName:'',lastName:'',image:'',id:''};
      this.task.dueDate = new Date();
    }
  }

  save() {
    const data = this.taskForm.value;    
    this.submitClicked.emit(data);
    this.dialogRef.close();
  }

  closeDialog() {
    this.dialogRef.close();
  }

  getClass(control:string)
  {
    return this.taskForm.controls[control].dirty && this.taskForm.controls[control].errors?
      'invalid-input':'';
  }

}
