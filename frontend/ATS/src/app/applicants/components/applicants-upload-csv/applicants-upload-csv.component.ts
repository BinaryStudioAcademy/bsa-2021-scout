import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FileType } from 'src/app/shared/enums/file-type.enum';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-applicants-upload-csv',
  templateUrl: './applicants-upload-csv.component.html',
  styleUrls: ['./applicants-upload-csv.component.scss'],
})
export class ApplicantsUploadCsvComponent {

  public allowedCvFileType = FileType.Csv;

  constructor(private readonly applicantsService: ApplicantsService,
    private modal: MatDialogRef<ApplicantsUploadCsvComponent>,
    private readonly notificationsService: NotificationService) { }

  public selectedFile: File | null = null;

  public uploadApplicantsCsv(files: File[]) {
    this.selectedFile = files[0];
  }

  public OnUpload() {
    if (this.selectedFile != null) {
      this.applicantsService.createApplicantsFromCSV(this.selectedFile).subscribe(values => {
        if(values.body?.length!=0){
          this.notificationsService
            .showSuccessMessage(`Successfully loaded ${values.body?.length} applicants`);
        }
        else{
          this.notificationsService.showInfoMessage('No loaded applicants');
        }
      },
      _ => (this.notificationsService.showErrorMessage('Failde to load data from csv')),
      );
    }
    this.modal.close();
  }
}
