import { Component, OnInit,Inject,Output,EventEmitter,ViewChild,AfterViewInit } 
  from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatSelect } from '@angular/material/select';
import { ApplicantIsSelected } from 'src/app/shared/models/applicants/applicant-select';

const DATA: ApplicantIsSelected[] = [];

@Component({
  selector: 'app-select-modal',
  templateUrl: './select-modal.component.html',
  styleUrls: ['./select-modal.component.scss',
    '../../../../task-management/components/modal/all-in-one/all-in-one.component.scss'],
})
export class SelectModalComponent implements OnInit, AfterViewInit {
  @Output() submitClicked = new EventEmitter<any>();
  applicants!: ApplicantIsSelected[];

  @ViewChild('select') select!: MatSelect;  
  
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<SelectModalComponent>,
  ) { }

  ngOnInit(): void {
    this.applicants = this.data.applicants;
  }

  ngAfterViewInit() {
    setTimeout(() => 
    //this.select.open(),
      20);
  }

  onKey(value:any) {
    if(!this.select.panelOpen) this.select.open();
    let filter = value.target.value.toLowerCase();
    let selection = this.select.value;
    this.applicants = (this.data.applicants as ApplicantIsSelected[]).filter(option => 
      (option.firstName.toLowerCase().startsWith(filter)) ||
      (option.lastName.toLowerCase().startsWith(filter)))
      .concat(this.data.selected);
    this.data.selected = selection;
    console.log(this.applicants.length);
  }

  compareById(o1:any, o2:any): boolean{    
    return o1.id === o2.id;
  }

  onUserChange(event:any) { 
    console.log(event);
  }

  removeUser(applicant:ApplicantIsSelected) {
    this.data.selected = (this.data.selected as ApplicantIsSelected[])
      .filter(x=>x.id !== applicant.id );
  }

  save() {    
    this.submitClicked.emit(this.data);
    this.dialogRef.close();
  }

  
}
