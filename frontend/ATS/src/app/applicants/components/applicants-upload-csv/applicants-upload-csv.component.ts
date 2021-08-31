import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FileType } from 'src/app/shared/enums/file-type.enum';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-applicants-upload-csv',
  templateUrl: './applicants-upload-csv.component.html',
  styleUrls: ['./applicants-upload-csv.component.scss'],
})
export class ApplicantsUploadCsvComponent {
  public allowedFileType = FileType.Csv;

  public selectedFile: File | null = null;

  constructor(private readonly applicantsService: ApplicantsService,
    private modal: MatDialogRef<ApplicantsUploadCsvComponent>,
    private readonly notificationsService: NotificationService,
    private router: Router) { }

  public uploadApplicantsCsv(files: File[]) {
    this.selectedFile = files[0];
  }

  public OnUpload() {
    this.modal.close();
    if (this.selectedFile) {
      this.applicantsService.getApplicantsFromCSV(this.selectedFile).subscribe(responce => {
        window.localStorage.setItem('csvFile', JSON.stringify(responce.body));
        this.router.navigate(['applicants/csv']);
      }, _ => (this.notificationsService.showErrorMessage('Failed to load data from csv')));
    }
  }
}
