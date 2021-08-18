import { Component, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';


@Component({
  selector: 'app-create-talentpool-modal',
  templateUrl: './create-talentpool-modal.component.html',
  styleUrls: ['./create-talentpool-modal.component.scss'],
})
export class CreateTalentpoolModalComponent {
  @Output() submitClicked = new EventEmitter<any>();

  public createPool: FormGroup = new FormGroup({
    'name': new FormControl('', [
      Validators.required,            
    ]),
    'description': new FormControl('', [
      Validators.required,
    ]),
  });

  constructor(public dialogRef: MatDialogRef<CreateTalentpoolModalComponent>) {}

  save() {
    const data = this.createPool.value;    
    this.submitClicked.emit(data);
    this.dialogRef.close();
  }

  closeDialog() {
    this.dialogRef.close();
  }

}
