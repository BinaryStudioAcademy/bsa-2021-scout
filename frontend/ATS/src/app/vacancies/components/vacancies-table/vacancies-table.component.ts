import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';

export enum VacancyStatus{
  Invited = 'Invited',
  Active = 'Active',
  Former = 'Former',
  Vacation = 'Vacation'
}

export interface VacancyData {
  title: string;
  candidates: string;
  department: string;
  responsible: string;
  created: Date;
  status: VacancyStatus;
}
​
​
const HRs: string[] = [
  'Livia Baptista',
  'Emery Culhain',
  'Mira Workham',
];
const NAMES: string[] = [
  'Interface Designer', 'Software Enginner', 'Project Manager', 'Developer', 'QA',
];

const STATUES: VacancyStatus[] = [
  VacancyStatus.Active,
  VacancyStatus.Former,
  VacancyStatus.Invited,
  VacancyStatus.Vacation,
];
​

@Component({
  selector: 'app-vacancies-table',
  templateUrl: './vacancies-table.component.html',
  styleUrls: ['./vacancies-table.component.scss'],
})
​
export class VacanciesTableComponent implements AfterViewInit {
  displayedColumns: string[] =
  ['position', 'title', 'candidates', 'department',
    'responsible', 'created', 'status', 'actions'];
  dataSource: MatTableDataSource<VacancyData>;
​
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
​
  constructor(private dialog: MatDialog) {
    const vacancies = Array.from({ length: 99 }, (_, k) => createNewVacancy());
​
    // Assign the data to the data source for the table to render
    this.dataSource = new MatTableDataSource(vacancies);
  }
​
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }
  ​
​
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }
​
  ​ 
}
​
/** Builds and returns a new User. */
function createNewVacancy(): VacancyData {
  const name = NAMES[Math.round(Math.random() * (NAMES.length - 1))];
​
  return {
    title: name,
    candidates: '10/10',
    responsible: HRs[Math.round(Math.random() * (HRs.length - 1))],
    department: 'Lorem ipsum dorot sit',
    created: new Date(),
    status: STATUES[Math.round(Math.random()*(STATUES.length - 1))],
  };
}