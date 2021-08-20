import {
  AfterViewInit,
  Component,
  ViewChild,
  ElementRef,
  ChangeDetectorRef,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { VacancyStatus } from 'src/app/shared/models/vacancy/vacancy-status';
import { VacancyData } from 'src/app/shared/models/vacancy/vacancy-data';
import { VacancyDataService } from 'src/app/shared/services/vacancy-data.service';
import { Router } from '@angular/router';
import { VacancyCreate } from 'src/app/shared/models/vacancy/vacancy-create';
import { EditVacancyComponent } from '../edit-vacancy/edit-vacancy.component';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

const HRs: string[] = ['Livia Baptista', 'Emery Culhain', 'Mira Workham'];
const NAMES: string[] = [
  'Interface Designer',
  'Software Enginner',
  'Project Manager',
  'Developer',
  'QA',
];

const STATUES: VacancyStatus[] = [
  VacancyStatus.Active,
  VacancyStatus.Former,
  VacancyStatus.Invited,
  VacancyStatus.Vacation,
];
@Component({
  selector: 'app-vacancies-table',
  templateUrl: './vacancies-table.component.html',
  styleUrls: ['./vacancies-table.component.scss'],
})
export class VacanciesTableComponent implements AfterViewInit {
  displayedColumns: string[] = [
    'position',
    'title',
    'candidates',
    'teamInfo',
    'responsible',
    'creationDate',
    'status',
    'project',
    'actions',
  ];

  dataSource: MatTableDataSource<VacancyData> =
  new MatTableDataSource<VacancyData>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild('input') serachField!: ElementRef;

  public loading: boolean = true;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private router: Router,
    private cd: ChangeDetectorRef,
    private dialog: MatDialog,
    private service: VacancyDataService,
  ) {
    this.getVacancies();
  }

  getVacancies() {
    this.service
      .getList()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((data) => {
        this.loading = false;
        this.dataSource.data = data;
        this.directive.applyFilter$.emit();
      });
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(EditVacancyComponent, {
      width: '914px',
      height: 'auto',
      data: {},
    });

    dialogRef.afterClosed().subscribe((_) => this.getVacancies());
  }

  onEdit(vacancyEdit: VacancyCreate): void {
    this.dialog.open(EditVacancyComponent, {
      data: {
        vacancyToEdit: vacancyEdit,
      },
    });
    this.dialog.afterAllClosed.subscribe((_) => this.getVacancies());
  }

  saveVacancy(changedVacancy: VacancyData) {
    this.dataSource.data.unshift(changedVacancy);
  }
  public getStatus(index: number): string {
    return VacancyStatus[index];
  }
  public toStagedReRoute(id: string) {
    this.router.navigateByUrl(`candidates/${id}`);
  }
  public clearSearchField() {
    this.serachField.nativeElement.value = '';
    this.dataSource.filter = '';
    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }
}
/** Builds and returns a new User. */
// function createNewVacancy(): VacancyData {
//   const name = NAMES[Math.round(Math.random() * (NAMES.length - 1))];
// ​
//   return {
//     title: name,
//     requiredCandidatesAmount: Math.round(Math.random()*4+1),
//     currentApplicantsAmount: Math.round(Math.random()*10 +1),
//     responsible: HRs[Math.round(Math.random() * (HRs.length - 1))],
//     department: 'Lorem ipsum dorot sit',
//     creationDate: new Date(),
//     status: STATUES[Math.round(Math.random()*(STATUES.length - 1))],
//   };
// }
