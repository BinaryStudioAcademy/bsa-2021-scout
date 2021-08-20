import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-project-delete-confirm',
  templateUrl: './project-delete-confirm.component.html',
  styleUrls: ['./project-delete-confirm.component.scss'],
})
export class ProjectDeleteConfirmComponent {

  constructor(private readonly dialogRef: MatDialogRef<ProjectDeleteConfirmComponent>)
  { }

  public sendDialogResponse(confirmDeletion: boolean): void {
    this.dialogRef.close(confirmDeletion);
  }

}
