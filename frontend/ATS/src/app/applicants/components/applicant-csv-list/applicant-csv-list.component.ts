import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { CreateApplicant } from 'src/app/shared/models/applicants/create-applicant';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { CsvApplicant } from '../../models/CsvApplicant';
import { ApplicantUpdateCsvComponent } 
  from '../applicant-update-csv/applicant-update-csv.component';

@Component({
  selector: 'app-applicant-csv-list',
  templateUrl: './applicant-csv-list.component.html',
  styleUrls: ['./applicant-csv-list.component.scss'],
})
export class ApplicantCsvListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'isValid', 'firstName', 'lastName', 'email', 'actions'];
  addedDisplayedColumns: string[] = ['position', 'firstName', 'lastName', 'email'];
  dataSource = new MatTableDataSource<CsvApplicant>();
  addedDataSource = new MatTableDataSource<CsvApplicant>();

  public allApplicants: CsvApplicant[] = [];
  public emails: string[] = [];
  public newApplicants: CsvApplicant[] = [];
  public addedApplicants: CsvApplicant[] = [];

  constructor(private readonly applicantsService: ApplicantsService,
    private readonly notificationsService: NotificationService,
    private readonly dialog: MatDialog) { }

  ngOnInit(): void {
    let csvFileJSON = window.localStorage.getItem('csvFile');

    if(csvFileJSON){
      this.allApplicants = JSON.parse(csvFileJSON);
    }
    
    window.localStorage.removeItem('csvFile');

    this.applicantsService.getApplicants().subscribe(applicants => {
      applicants.forEach(applicant => {
        this.emails.push(applicant.email);

        this.allApplicants.forEach(applicantCsv => {
          if (applicant.email == applicantCsv.email) {
            applicantCsv.isExist = true;
          }
        });

      });

      this.allApplicants.forEach(applicantFirst => {
        this.allApplicants.forEach(applicantSecond => {
          if (applicantFirst != applicantSecond && applicantFirst.email == applicantSecond.email) {
            applicantFirst.isRepeat = true;
            applicantSecond.isRepeat = true;
          }
        });
      });

      this.allApplicants?.forEach(applicant => {
        this.newApplicants?.push(applicant);
      });

      this.dataSource.data = this.newApplicants;
      this.sortDataSource();
    });
  }

  public onCreate(csvApplicant: CsvApplicant) {
    let applicant: CreateApplicant = new CreateApplicant(csvApplicant);

    this.applicantsService.addApplicant(applicant)
      .subscribe(
        _ => {
          this.newApplicants?.forEach((element, index) => {
            if (element == csvApplicant) {
              this.addedApplicants?.push(element);
              this.newApplicants?.splice(index, 1);
            }
          });

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
      applicant.isValid && !applicant.isRepeat);

    let createApplicants: CreateApplicant[] = [];

    createApplicantsCsv?.forEach(applicant => {
      createApplicants.push(new CreateApplicant(applicant));
    });

    this.applicantsService.addRangeApplicants(createApplicants ? createApplicants : [])
      .subscribe(_ => {
        this.newApplicants?.filter(applicant =>
          applicant.isValid && !applicant.isRepeat && !applicant.isExist)
          .forEach(element => {
            this.addedApplicants?.push(element);
          });

        this.newApplicants = this.newApplicants?.filter(applicant =>
          !applicant.isValid || applicant.isRepeat || applicant.isExist)!;

        this.dataSource.data = this.newApplicants;
        this.addedDataSource.data = this.addedApplicants;
        this.sortDataSource();
      });
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
        editedApplicant.isRepeat = false;

        if (email != editedApplicant.email) {
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

        this.newApplicants?.splice(index, 1, editedApplicant);
        this.dataSource.data = this.newApplicants;
        this.sortDataSource();
      }
    });
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
