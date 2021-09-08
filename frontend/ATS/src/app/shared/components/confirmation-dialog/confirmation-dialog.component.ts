import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.scss'],
})
export class ConfirmationDialogComponent {
  public action: string = '';
  public entityName: string = '';
  public additionalMessage: string = '';

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: 
    {action: string, entityName: string, additionalMessage: string },
    private readonly dialogRef: MatDialogRef<ConfirmationDialogComponent>) {
    this.action = data.action;
    this.entityName = data.entityName;
    this.additionalMessage = data.additionalMessage;
  }

  public sendDialogResponse(confirmArchivation: boolean): void {
    this.dialogRef.close(confirmArchivation);
  }
}
