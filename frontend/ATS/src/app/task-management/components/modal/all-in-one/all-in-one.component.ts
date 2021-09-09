import { Component, Output, EventEmitter,Inject, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import { ApplicantShort } from 'src/app/shared/models/task-management/applicant-short';
import { Task } from 'src/app/shared/models/task-management/task';
import { UserInfo } from 'src/app/shared/models/task-management/user-short';
import { AbstractControl, ValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { startWith,map } from 'rxjs/operators';
import { isString } from 'lodash';
import moment from 'moment';

@Component({
  selector: 'app-all-in-one',
  templateUrl: './all-in-one.component.html',
  styleUrls: ['./all-in-one.component.scss'],
})
export class AllInOneComponent implements OnInit {
  @Output() submitClicked = new EventEmitter<any>();
  public task: Task = {} as Task;
  public selectedUsers: UserInfo[] = [];
  
  
  public applicantsOptions!: Observable<any[]>;
  public applicants: ApplicantShort[]=[];  
  public formType:string = '';
  public usersToched : boolean = false;

  public taskForm: FormGroup = new FormGroup({
    'name': new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(100)]),
    'isDone': new FormControl(''),
    'applicant': new FormControl('', [
      Validators.required,
      this.RequireMatch]),
    'dueDate': new FormControl('',[
      Validators.required]),      
    'note': new FormControl(''),
  });
  

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<AllInOneComponent>,
  ) {}
  

  
  private _filter(value: string): ApplicantShort[] {
    const filterValue = isString(value) ? value.toLowerCase() : '';

    return this.applicants.filter(x => x.firstName.toLowerCase().includes(filterValue));
  }

  displayWith(obj?: any): string {
    return obj ? `${obj.firstName} ${obj.lastName}` : '';
  }

  compareById(o1:any, o2:any): boolean{    
    return o1.id === o2.id;
  }

  ngOnInit() {

    this.applicantsOptions = this.taskForm.controls.applicant.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value)),
      );

        
    this.applicants = this.data.applicants;
        

    if(this.data.task.id) {
      this.formType = 'Edit';
      this.task = this.data.task;
    }
    else {
      this.formType = 'Create';      
    }
  }

  removeUser(user:UserInfo) {
    this.usersToched = true;
    let index = this.task.teamMembers.findIndex(x=> x.id == user.id);
    if(index>-1) {      
      this.task.teamMembers = this.task.teamMembers.slice(0,index)
        .concat(this.task.teamMembers.slice(index+1));      
    }
  }

  onUserChange(event:any) {
    this.usersToched = true;
  }

  
  save() {
    const data = this.taskForm.value;
    data.dueDate = moment(data.dueDate).add(moment(data.dueDate).utcOffset(), 'minutes');
    console.log(moment(data.dueDate).utcOffset());
    data.teamMembers = this.task.teamMembers;
    data.id = this.task.id ? this.task.id:'';
    this.submitClicked.emit(data);
    this.dialogRef.close(data);
  }

  closeDialog() {
    this.dialogRef.close();
  }

  getClass(control:string)
  {
    return this.taskForm.controls[control].dirty && this.taskForm.controls[control].errors?
      'invalid-input':'';
  }

  editMode()
  {
    return this.formType == 'Edit'?
      'edit':'create';
  }

  
  RequireMatch(control: AbstractControl) {
    const selection: any = control.value;
    if (typeof selection === 'string') {
      return { incorrect: true };
    }
    return null;
  }
  
}
