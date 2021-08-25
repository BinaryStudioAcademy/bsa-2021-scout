import {
  AfterViewInit, Component,
  ViewChild, ElementRef, ChangeDetectorRef, OnDestroy,
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
import { property } from 'lodash';
import { DeleteConfirmComponent } 
  from 'src/app/shared/components/delete-confirm/delete-confirm.component';

import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';


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
export class VacanciesTableComponent implements AfterViewInit, OnDestroy {
  displayedColumns: string[] =
  ['position', 'title', 'candidatesAmount', 'responsible', 'teamInfo', 
    'project', 'creationDate', 'actions'];
  dataSource: MatTableDataSource<VacancyData> = new MatTableDataSource<VacancyData>();
  mainData!: VacancyData[];
  isFollowedPage: boolean = false;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild('input') serachField!: ElementRef;

  public loading: boolean = true;

  constructor(
    private router: Router, private cd: ChangeDetectorRef,
    private dialog: MatDialog, private service: VacancyDataService,
    private notificationService: NotificationService,
  ) {
    this.getVacancies();
  }

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  getVacancies() {
    this.service
      .getList()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        data => {
          this.dataSource.data = data;
          data.forEach((d, i) => {
            d.position = i + 1;
          });
          this.mainData = data;
          this.directive.applyFilter$.emit();
        },
        () => {
          this.loading = false;
          this.notificationService.showErrorMessage('Failed to get vacancies.');
        },
      );
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(EditVacancyComponent, {
      width: '914px',
      height: 'auto',
      data: {},
    });

    dialogRef.afterClosed().subscribe(() => this.getVacancies());
  }
  public switchToFollowed(){
    this.isFollowedPage = true;
    this.dataSource.data = this.dataSource.data.filter(vacancy=>vacancy.isFollowed);
    this.directive.applyFilter$.emit();
  }
  public switchAwayToAll(){
    this.isFollowedPage = false;
    this.dataSource.data = this.mainData;
    this.directive.applyFilter$.emit();
  }
  public onBookmark(data: VacancyData, perfomToFollowCleanUp: boolean = false){
    let vacancyIndex:number = this.dataSource.data.findIndex(vacancy=>vacancy.id === data.id)!;
    data.isFollowed = !data.isFollowed;
    this.dataSource.data[vacancyIndex] = data;
    if(perfomToFollowCleanUp){
      this.dataSource.data = this.dataSource.data.filter(vacancy=>vacancy.isFollowed);
    }
    this.directive.applyFilter$.emit();
  }
  onEdit(vacancyEdit: VacancyCreate): void {
    this.dialog.open(EditVacancyComponent, {
      data: {
        vacancyToEdit: vacancyEdit,
      },
    });
    this.dialog.afterAllClosed.subscribe(() => this.getVacancies());
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

  public showDeleteConfirmDialog(vacancyToDelete: VacancyData): void {
    const dialogRef = this.dialog.open(DeleteConfirmComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data:{
        entityName: 'Vacancy',
      },
    });

    dialogRef.afterClosed()
      .subscribe((response: boolean) => {
        if (response) {
          this.loading = true;
          this.service.deleteVacancy(vacancyToDelete.id)
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe(_ => {
              this.loading = false;
              this.notificationService
                .showSuccessMessage(`Vacancy ${vacancyToDelete.title} deleted!`);
              this.getVacancies();
            }, ()=>{
              this.loading = false;
            });
        }
      });
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
