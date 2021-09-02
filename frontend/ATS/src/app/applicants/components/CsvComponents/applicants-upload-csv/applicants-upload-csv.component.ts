import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CsvFile } from 'src/app/applicants/models/CsvFile';
import { FileType } from 'src/app/shared/enums/file-type.enum';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { CsvApplicant } from '../../../models/CsvApplicant';
import { ApplicantCsvService } from '../../../services/applicant-csv.service';

@Component({
  selector: 'app-applicants-upload-csv',
  templateUrl: './applicants-upload-csv.component.html',
  styleUrls: ['./applicants-upload-csv.component.scss'],
})
export class ApplicantsUploadCsvComponent {
  public allowedFileType = FileType.Csv;
  public selectedFile: File | null = null;
  
  constructor(private modal: MatDialogRef<ApplicantsUploadCsvComponent>,
    private readonly notificationsService: NotificationService,
    private readonly applicantsCsvService: ApplicantCsvService,
    private router: Router) { }

  public uploadApplicantsCsv(files: File[]) {
    this.selectedFile = files[0];
  }

  public OnUpload() {
    this.modal.close();
    if (this.selectedFile) {
      this.applicantsCsvService.getApplicantsFromCSV(this.selectedFile).subscribe(responce => {

        let applicants: CsvApplicant[] | null = responce.body;

        if(applicants != null && applicants.length != 0)
        {
          applicants?.forEach(applicant => {
            applicant.isAdded = false;
            applicant.creationDate = new Date();
            applicant.user = null;
          });
  
          let csvFile: CsvFile = {
            id: '',
            name: this.selectedFile?.name ? this.selectedFile!.name : '',
            json: JSON.stringify(applicants),
            dateAdded: new Date(),
            user: null,
          };
  
          
          this.applicantsCsvService.postCsvFile(csvFile).subscribe(file => {
            window.localStorage.setItem('csvFile', JSON.stringify(file.body));
            this.router.navigate(['applicants/csv']);
          });
        }
        else
        {
          this.notificationsService.showInfoMessage('This file is empty');
        }
      }, _ => (this.notificationsService.showErrorMessage('Failed to load data from csv')));
    }
  }
}
