import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-applicant-delete-confirm',
  templateUrl: './applicant-delete-confirm.component.html',
  styleUrls: ['./applicant-delete-confirm.component.scss'],
})
export class ApplicantDeleteConfirmComponent {

  constructor(private readonly dialogRef: MatDialogRef<ApplicantDeleteConfirmComponent>)
  { }

  public sendDialogResponse(confirmDeletion: boolean): void {
    this.dialogRef.close(confirmDeletion);
  }
}
