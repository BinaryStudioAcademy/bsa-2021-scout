import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Applicant } from 'src/app/shared/models/applicant';
import { CreateApplicantComponent } from '../create-applicant/create-applicant.component';
import { UpdateApplicantComponent } from '../update-applicant/update-applicant.component';

export type FullApplicant = Applicant
& { rate: number }
& { experience: number }

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

  public dataSource = new MatTableDataSource([
    { id: '2', firstName: 'sfsaf', lastName: 'sdsfff', middleName: 'gfdgggg',
      email: 'dsg@jhgjgh', rate: 3, vacancies: [ 
        { title: 'UI/UX', stage: 'Pre-offer' },
        { title: '.Net Dev', stage: 'Hot' },
        { title: 'Frontend', stage: 'Declined' },
      ], tags: [ 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA' ],
      experience: 1.2},
    { id: '2', firstName: 'sfsaf', lastName: 'sdsfff', middleName: 'gfdgggg',
      email: 'dsg@jhgjgh', rate: 3, vacancies: [ 
        { title: 'UI/UX', stage: 'Pre-offer' },
        { title: '.Net Dev', stage: 'Hot' },
        { title: 'Frontend', stage: 'Declined' },
      ], tags: [ 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA' ],
      experience: 1.2},
    { id: '2', firstName: 'sfsaf', lastName: 'sdsfff', middleName: 'gfdgggg',
      email: 'dsg@jhgjgh', rate: 3, vacancies: [ 
        { title: 'UI/UX', stage: 'Pre-offer' },
        { title: '.Net Dev', stage: 'Hot' },
        { title: 'Frontend', stage: 'Declined' },
      ], tags: [ 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA' ],
      experience: 1.2},
    { id: '2', firstName: 'sfsaf', lastName: 'sdsfff', middleName: 'gfdgggg',
      email: 'dsg@jhgjgh', rate: 3, vacancies: [ 
        { title: 'UI/UX', stage: 'Pre-offer' },
        { title: '.Net Dev', stage: 'Hot' },
        { title: 'Frontend', stage: 'Declined' },
      ], tags: [ 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA' ],
      experience: 1.2},
    { id: '2', firstName: 'sfsaf', lastName: 'sdsfff', middleName: 'gfdgggg',
      email: 'dsg@jhgjgh', rate: 3, vacancies: [ 
        { title: 'UI/UX', stage: 'Pre-offer' },
        { title: '.Net Dev', stage: 'Hot' },
        { title: 'Frontend', stage: 'Declined' },
      ], tags: [ 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA' ],
      experience: 1.2},
    { id: '2', firstName: 'sfsaf', lastName: 'sdsfff', middleName: 'gfdgggg',
      email: 'dsg@jhgjgh', rate: 3, vacancies: [ 
        { title: 'UI/UX', stage: 'Pre-offer' },
        { title: '.Net Dev', stage: 'Hot' },
        { title: 'Frontend', stage: 'Declined' },
      ], tags: [ 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA' ],
      experience: 1.2},
    { id: '2', firstName: 'sfsaf', lastName: 'sdsfff', middleName: 'gfdgggg',
      email: 'dsg@jhgjgh', rate: 3, vacancies: [ 
        { title: 'UI/UX', stage: 'Pre-offer' },
        { title: '.Net Dev', stage: 'Hot' },
      ], tags: [ 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA' ],
      experience: 1.2},
    { id: '2', firstName: 'sfsaf', lastName: 'sdsfff', middleName: 'gfdgggg',
      email: 'dsg@jhgjgh', rate: 3, vacancies: [ 
        { title: 'UI/UX', stage: 'Pre-offer' },
        { title: '.Net Dev', stage: 'Hot' },
        { title: 'Frontend', stage: 'Declined' },
      ], tags: [ 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA', 'Job', 'Offer', 'BSA' ],
      experience: 1.2},
  ]);

  public searchValue = '';

  constructor(private dialog: MatDialog)
  {}

  public showStarIcon(rateValue: number, applicantRate: number): string {
    return rateValue <= applicantRate
      ? 'star'
      : 'star_border';
  }

  public changeRate(applicant: FullApplicant, rateValue: number): void {
    applicant.rate = rateValue;
  }

  public showApplicantCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateApplicantComponent, {
      width: '914px',
      height: 'min-content',
      autoFocus: false,
    });

    dialogRef.afterClosed().subscribe();
  }

  public showApplicantUpdateDialog(applicant: FullApplicant): void {
    const dialogRef = this.dialog.open(UpdateApplicantComponent, {
      width: '914px',
      height: 'min-content',
      autoFocus: false,
      data: applicant,
    });

    dialogRef.afterClosed().subscribe();
  }

  public deleteApplicant(): void {
        
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
    this.dataSource.data = (this.dataSource.data as FullApplicant[]).sort((a, b) => {
      const isAsc = sort.direction === 'asc';
            
      switch (sort.active) {
        case 'name':
          return this.compareRows(a.firstName + ' ' + a.lastName + ' ' + a.middleName,
            b.firstName + ' ' + b.lastName + ' ' + b.middleName, isAsc);
        case 'rate':
          return this.compareRows(a.rate, b.rate, isAsc);
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