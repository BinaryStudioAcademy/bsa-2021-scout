import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-delete-confirm',
  templateUrl: './delete-confirm.component.html',
  styleUrls: ['./delete-confirm.component.scss'],
})
export class DeleteConfirmComponent {

  public entityName: string = '';

  constructor(@Inject(MAT_DIALOG_DATA) public data: { entityName: string },
    private readonly dialogRef: MatDialogRef<DeleteConfirmComponent>) {
    this.entityName = data.entityName;
  }

  public sendDialogResponse(confirmDeletion: boolean): void {
    this.dialogRef.close(confirmDeletion);
  }

}
