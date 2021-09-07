import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { CreateApplicant } from 'src/app/shared/models/applicants/create-applicant';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { CsvApplicant } from '../../../models/CsvApplicant';
import { CsvFile } from '../../../models/CsvFile';
import { ApplicantCsvService } from '../../../services/applicant-csv.service';
import { ApplicantUpdateCsvComponent }
  from '../../applicant-update-csv/applicant-update-csv.component';

@Component({
  selector: 'app-applicant-csv-file-tables',
  templateUrl: './applicant-csv-file-tables.component.html',
  styleUrls: ['./applicant-csv-file-tables.component.scss'],
})
export class ApplicantFileTablesComponent implements OnInit {
  displayedColumns: string[] = ['position', 'isValid', 'firstName', 'lastName', 'email', 'actions'];
  addedDisplayedColumns: string[] = 
  ['position', 'firstName', 'lastName', 'email', 'dateAdded', 'user'];
  dataSource = new MatTableDataSource<CsvApplicant>();
  addedDataSource = new MatTableDataSource<CsvApplicant>();

  public emails: string[] = [];
  public newApplicants: CsvApplicant[] = [];
  public addedApplicants: CsvApplicant[] = [];
  public csvFile: CsvFile = new CsvFile();

  constructor(
    private readonly applicantsService: ApplicantsService,
    private readonly notificationsService: NotificationService,
    private readonly applicantsCsvService: ApplicantCsvService,
    private router: Router,
    private readonly dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    let csvFileJSON = window.localStorage.getItem('csvFile');
    let allApplicants: CsvApplicant[] = [];

    if (csvFileJSON) {
      this.csvFile = JSON.parse(csvFileJSON);
      allApplicants = JSON.parse(this.csvFile.json);

      allApplicants?.forEach(applicant => {
        if (applicant.isAdded) {
          this.addedApplicants.push(applicant);
        }
        else {
          this.newApplicants?.push(applicant);
        }
      });

      this.newApplicants.forEach(applicant => {
        applicant.isExist = false;
        applicant.isRepeat = false;
      });
  
      window.localStorage.removeItem('csvFile');
  
      this.applicantsService.getApplicants().subscribe(applicants => {
        applicants.forEach(applicant => {
          this.emails.push(applicant.email);
  
          this.newApplicants.forEach(applicantCsv => {
            if (applicant.email == applicantCsv.email) {
              applicantCsv.isExist = true;
            }
          });
        });
  
        this.newApplicants.forEach(applicantFirst => {
          this.newApplicants.forEach(applicantSecond => {
            if (applicantFirst != applicantSecond && 
          applicantFirst.email == applicantSecond.email) 
            {
              applicantFirst.isRepeat = true;
              applicantSecond.isRepeat = true;
            }
          });
        });
  
        this.dataSource.data = this.newApplicants;
        this.addedDataSource.data = this.addedApplicants;
        this.sortDataSource();
      });
    }
    else {
      this.router.navigate(['applicants/csvfiles']);
    }
  }

  public onCreate(csvApplicant: CsvApplicant) {
    let applicant: CreateApplicant = new CreateApplicant(csvApplicant);

    this.applicantsCsvService.addCsvApplicant(applicant)
      .subscribe(
        applicant => {
          this.newApplicants?.forEach((element, index) => {
            if (element == csvApplicant) {
              element.isAdded = true;
              element.creationDate = applicant.creationDate;
              element.user = applicant.user;
              this.addedApplicants?.push(element);
              this.newApplicants?.splice(index, 1);
            }
          });

          this.SaveNewApplicants();
          this.dataSource.data = this.newApplicants;
          this.addedDataSource.data = this.addedApplicants;
          this.sortDataSource();
        },
        (error: Error) => {
          this.notificationsService.showErrorMessage(
            error.message,
            'Cannot create an applicant',
          );
        },
      );
  }

  public onCreateRange() {
    let createApplicantsCsv = this.newApplicants?.filter(applicant =>
      applicant.isValid && !applicant.isRepeat && !applicant.isExist);

    let createApplicants: CreateApplicant[] = [];

    createApplicantsCsv?.forEach(applicant => {
      createApplicants.push(new CreateApplicant(applicant));
    });

    if(createApplicants != null && createApplicants.length != 0){
      this.applicantsCsvService.addRangeCsvApplicants(createApplicants ? createApplicants : [])
        .subscribe(createdApplicants => {

          this.newApplicants.forEach(applicant => {
            if (applicant.isValid && !applicant.isRepeat && !applicant.isExist) {
              createdApplicants.body?.forEach(createdApplicant => {
                if (applicant.email == createdApplicant.email) {
                  applicant.creationDate = createdApplicant.creationDate;
                  applicant.user = createdApplicant.user;
                  applicant.isAdded = true;
                  this.addedApplicants?.push(applicant);
                }
              });
            }
          });

          this.newApplicants = this.newApplicants?.filter(applicant =>
            !applicant.isValid || applicant.isRepeat || applicant.isExist)!;

          this.SaveNewApplicants();
          this.dataSource.data = this.newApplicants;
          this.addedDataSource.data = this.addedApplicants;
          this.sortDataSource();
        });
    }
    else
    {
      this.notificationsService.showInfoMessage('There is no valid applicants');
    }
  }

  public onEdit(csvApplicant: CsvApplicant, index: number) {
    let email: string = csvApplicant.email;

    const dialogRef = this.dialog.open(ApplicantUpdateCsvComponent, {
      width: '532px',
      autoFocus: false,
      data: csvApplicant,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != undefined && result != '') {
        let editedApplicant: CsvApplicant = result?.data;

        if (email != editedApplicant.email) {
          editedApplicant.isRepeat = false;
          editedApplicant.isExist = false;

          this.newApplicants?.forEach((element) => {
            if (element != csvApplicant && element.email == editedApplicant.email) {
              element.isRepeat = true;
              editedApplicant.isRepeat = true;
            }
          });

          this.emails.forEach(email => {
            if (email == editedApplicant.email) {
              editedApplicant.isExist = true;
            }
          });

          this.addedApplicants?.forEach((element) => {
            if (element != csvApplicant && element.email == editedApplicant.email) {
              editedApplicant.isRepeat = true;
            }
          });

          let count: number = 0;
          this.newApplicants?.forEach(element => {
            if (element != csvApplicant && element.email == email) {
              count++;
            }
          });

          if (count == 1) {
            this.newApplicants?.forEach(element => {
              if (element != csvApplicant && element.email == email) {
                element.isRepeat = false;
              }
            });
          }
        }

        this.SaveNewApplicants();
        this.newApplicants?.splice(index, 1, editedApplicant);
        this.dataSource.data = this.newApplicants;
        this.sortDataSource();
      }
    });
  }

  SaveNewApplicants() {
    let applicantsToSave: CsvApplicant[] = [];
    applicantsToSave = [...this.newApplicants, ...this.addedApplicants];
    this.csvFile.json = JSON.stringify(applicantsToSave);
    this.applicantsCsvService.putCsvFile(this.csvFile).subscribe();
  }

  sortDataSource() {
    this.dataSource.data = this.dataSource.data.sort((a: CsvApplicant, b: CsvApplicant) => {
      if (a.firstName + a.lastName < b.firstName + b.lastName) {
        return -1;
      } else if (a.firstName + a.lastName > b.firstName + b.lastName) {
        return 1;
      } else {
        return 0;
      }
    });

    this.dataSource.data = this.dataSource.data.sort((a: CsvApplicant, b: CsvApplicant) => {
      if (a.isExist < b.isExist) {
        return -1;
      } else if (a.isExist > b.isExist) {
        return 1;
      } else {
        return 0;
      }
    });

    this.addedDataSource.data = this.addedDataSource.data
      .sort((a: CsvApplicant, b: CsvApplicant) => {
        if (a.firstName + a.lastName < b.firstName + b.lastName) {
          return -1;
        } else if (a.firstName + a.lastName > b.firstName + b.lastName) {
          return 1;
        } else {
          return 0;
        }
      });
  }

  applyFilter(event: Event, dataSource: MatTableDataSource<CsvApplicant>) {
    const filterValue = (event.target as HTMLInputElement).value;
    dataSource.filter = filterValue.trim().toLowerCase();
  }
}
