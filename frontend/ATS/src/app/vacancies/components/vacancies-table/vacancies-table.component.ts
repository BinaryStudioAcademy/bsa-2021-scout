import {
  AfterViewInit, Component,
  ViewChild, ElementRef, ChangeDetectorRef,
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


const STATUES: VacancyStatus[] = [
  VacancyStatus.Active,
  VacancyStatus.Former,
  VacancyStatus.Invited,
  VacancyStatus.Vacation,
];

export interface IIndexable {
  [key: string]: any;
}

@Component({
  selector: 'app-vacancies-table',
  templateUrl: './vacancies-table.component.html',
  styleUrls: ['./vacancies-table.component.scss'],
})
export class VacanciesTableComponent implements AfterViewInit {
  displayedColumns: string[] =
  ['position', 'title', 'candidatesAmount', 'responsible', 'project', 
    'teamInfo', 'creationDate', 'status', 'actions'];
  dataSource: MatTableDataSource<VacancyData> = new MatTableDataSource<VacancyData>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild('input') serachField!: ElementRef;

  public loading: boolean = true;

  constructor(private router:Router, private cd: ChangeDetectorRef,
    private dialog: MatDialog, private service: VacancyDataService){
    service.getList().pipe(takeUntil(this.unsubscribe$))
      .subscribe(data=>{
        this.getVacancies();
      });
  }

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  getVacancies() {
    this.service.getList().subscribe(data => {
      this.dataSource.data = data;
      this.loading = false;
      data.forEach((d, i) => {
        d.position = i + 1;
      });
      this.directive.applyFilter$.emit();
    },
    );
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
    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'project': return item.project.name;
        case 'teamInfo': return item.project.teamInfo;
        case 'responsible': return item.responsibleHr.firstName + ' ' + item.responsibleHr.lastName;
        default: return (item as IIndexable)[property];

      }
    };
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
