import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CsvFile } from '../../../models/CsvFile';
import { ApplicantCsvService } from '../../../services/applicant-csv.service';
import { ApplicantsUploadCsvComponent } 
  from '../applicants-upload-csv/applicants-upload-csv.component';

@Component({
  selector: 'app-applicant-csv-files-list',
  templateUrl: './applicant-csv-files-list.component.html',
  styleUrls: ['./applicant-csv-files-list.component.scss'],
})
export class ApplicantCsvFilesListComponent {

  public csvFiles: CsvFile[] | null = [];

  constructor(
    private readonly applicantsCsvService: ApplicantCsvService,
    private readonly dialog: MatDialog,
  ) {
    this.applicantsCsvService.getCsvFiles().subscribe(files => {
      this.csvFiles = files.body;
    });
  }

  public showUploadCSVDialog(): void{
    this.dialog.open(ApplicantsUploadCsvComponent, {
      width: '600px',
      panelClass: 'applicants-csv-modal',
      autoFocus: false,
    });
  }
  
}
