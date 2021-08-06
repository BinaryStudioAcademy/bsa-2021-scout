import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { applicants } from '../../applicants-test';
import { CreateApplicantComponent } from '../create-applicant/create-applicant.component';
import { UpdateApplicantComponent } from '../update-applicant/update-applicant.component';

const isTestMode = true;

@Component({
  selector: 'app-applicants',
  templateUrl: 'applicants.component.html',
  styleUrls: [ 'applicants.component.scss' ],
})

export class ApplicantsComponent {
  public displayedColumns: string[] = [
    'name',
    'rate',
    'email',
    'active_vacancies',
    'jobs_list',
    'tags',
    'status',
    'control_buttons',
  ];

  public searchValue = '';
  public dataSource = new MatTableDataSource<Applicant>();

  private $unsubscribe = new Subject();

  constructor(
    private readonly dialog: MatDialog,
    private readonly notificationsService: NotificationService,
    private readonly httpCLient: HttpClientService,
  ) {
    if (isTestMode) {
      this.dataSource.data = applicants;
    }
    else {
      // Will be the code of the controller after applicants backend will be implemented
      this.httpCLient.getRequest<Applicant[]>('/applicants')
        .pipe(
          takeUntil(this.$unsubscribe),
        )
        .subscribe((result: Applicant[]) => {
          this.dataSource.data = result;
        },
        (error: Error) => {
          this.notificationsService.showErrorMessage(error.message,
            'Cannot download applicants from the host');
        });
    }
  }

  public showApplicantCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateApplicantComponent, {
      width: '914px',
      height: 'min-content',
      autoFocus: false,
    });

    dialogRef.afterClosed()
      .subscribe((result: Applicant) => {
        if (result) {
          this.dataSource.data.unshift(result);
          this.notificationsService.showSuccessMessage('An applicant was succesfully created',
            'Success!');
        }
      });
  }

  public showApplicantUpdateDialog(applicant: Applicant): void {
    const dialogRef = this.dialog.open(UpdateApplicantComponent, {
      width: '914px',
      height: 'min-content',
      autoFocus: false,
      data: applicant,
    });

    dialogRef.afterClosed()
      .subscribe((result: Applicant) => {
        if (result) {
          let applicantIndex = this.dataSource.data.findIndex(a => a.id === result.id);
          this.dataSource.data[applicantIndex] = result;

          this.notificationsService.showSuccessMessage('An applicant was succesfully updated',
            'Success!');
        }
      });
  }

  public deleteApplicant(applicantId: string): void {
    this.httpCLient.deleteRequest('/applicants/' + applicantId)
      .pipe(
        takeUntil(this.$unsubscribe),
      )
      .subscribe(() => {
        let applicantIndex = this.dataSource.data.findIndex(a => a.id === applicantId);
        this.dataSource.data.slice(applicantIndex, 1);

        this.notificationsService.showSuccessMessage('The applicant was succesfully deleted',
          'Success!');
      },
      (error: Error) => {
        this.notificationsService.showErrorMessage(error.message,
          'Cannot delete the applicant');
      });
  }

  public clearInput(): void {
    this.searchValue = '';
    this.dataSource.filter = '';
  }

  public applyFilter(event: Event): void {
    const fullName = (event.target as HTMLInputElement).value;
    this.dataSource.filter = fullName.trim().toLowerCase();
  }

  public sortData(sort: Sort): void {
    this.dataSource.data = (this.dataSource.data as Applicant[]).sort((a, b) => {
      const isAsc = sort.direction === 'asc';
                  
      switch (sort.active) {
        case 'name':
          return this.compareRows(a.firstName + ' ' + a.lastName + ' ' + a.middleName,
            b.firstName + ' ' + b.lastName + ' ' + b.middleName, isAsc);
        case 'email':
          return this.compareRows(a.email, b.email, isAsc);
        case 'active_vacancies':
          return this.compareRows(a.vacancies.length, b.vacancies.length, isAsc);
        case 'jobs_list':
          return this.compareRows(a.vacancies.length, b.vacancies.length, isAsc);
        case 'tags':
          return this.compareRows(a.tags.length, b.tags.length, isAsc);
        default:
          return 0;
      }
    });
  }

  private compareRows(a: number|string, b: number|string, isAsc: boolean): number {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
}