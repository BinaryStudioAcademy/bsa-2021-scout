import { Subject } from 'rxjs';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { takeUntil } from 'rxjs/operators';
import { Sort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { CreateApplicantComponent } from '../create-applicant/create-applicant.component';
import { UpdateApplicantComponent } from '../update-applicant/update-applicant.component';
import { MatPaginator } from '@angular/material/paginator';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { Tag } from 'src/app/shared/models/tags/tag';
import { ActivatedRoute } from '@angular/router';
import { CreateApplicant } from 'src/app/shared/models/applicant/create-applicant';

@Component({
  selector: 'app-applicants',
  templateUrl: 'applicants.component.html',
  styleUrls: ['applicants.component.scss', '../../common/scroll.scss'],
})
export class ApplicantsComponent implements OnInit, AfterViewInit {
  public displayedColumns: string[] = [
    'name',
    'rate',
    'email',
    'active_vacancies',
    'jobs_list',
    'status',
    'tags',
    'control_buttons',
  ];

  public dataSource = new MatTableDataSource<Applicant>();
  public cashedData: Applicant[] = [];
  public creationData?: CreateApplicant;

  public isShowAllTags = false;

  public searchValue = '';
  @ViewChild(MatPaginator) public paginator: MatPaginator | undefined =
  undefined;
  @ViewChild(StylePaginatorDirective) public directive:
  | StylePaginatorDirective
  | undefined = undefined;

  private $unsubscribe = new Subject();

  constructor(
    private readonly dialog: MatDialog,
    private readonly notificationsService: NotificationService,
    private readonly applicantsService: ApplicantsService,
    private readonly route: ActivatedRoute,
  ) {}

  public ngOnInit(): void {
    this.route.queryParams.subscribe((query) => {
      if (query['data']) {
        const creationData: CreateApplicant = JSON.parse(atob(query['data']));
        this.creationData = creationData;
        this.showApplicantCreateDialog();
      }
    });

    this.applicantsService
      .getApplicants()
      .pipe(takeUntil(this.$unsubscribe))
      .subscribe(
        (result: Applicant[]) => {
          this.dataSource.data = result;
          this.cashedData = result;
        },
        (error: Error) => {
          this.notificationsService.showErrorMessage(
            error.message,
            'Cannot download applicants from the host',
          );
        },
      );
  }

  public ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator!;
    this.dataSource.filter = this.searchValue.trim().toLowerCase();
  }

  public showApplicantCreateDialog(): void {
    const creationData: CreateApplicant | undefined = this.creationData;
    this.creationData = undefined;

    const dialogRef = this.dialog.open(CreateApplicantComponent, {
      width: '914px',
      height: 'min-content',
      autoFocus: false,
      data: creationData,
    });

    dialogRef.afterClosed().subscribe((result: Applicant) => {
      if (result) {
        this.cashedData.unshift(result);
        this.dataSource.data = this.cashedData;

        this.notificationsService.showSuccessMessage(
          'An applicant was succesfully created',
          'Success!',
        );
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

    dialogRef.afterClosed().subscribe((result: Applicant) => {
      if (result) {
        let applicantIndex = this.cashedData.findIndex(
          (a) => a.id === result.id,
        );
        this.cashedData[applicantIndex] = result;
        this.dataSource.data = this.cashedData;

        this.notificationsService.showSuccessMessage(
          'An applicant was succesfully updated',
          'Success!',
        );
      }
    });
  }

  public deleteApplicant(applicantId: string): void {
    this.applicantsService
      .deleteApplicant(applicantId)
      .pipe(takeUntil(this.$unsubscribe))
      .subscribe(
        () => {
          let applicantIndex = this.dataSource.data.findIndex(
            (a) => a.id === applicantId,
          );

          this.cashedData.splice(applicantIndex, 1);
          this.dataSource.data = this.cashedData;
          this.notificationsService.showSuccessMessage(
            'The applicant was successfully deleted',
            'Success!',
          );
        },
        (error: Error) => {
          this.notificationsService.showErrorMessage(
            error.message,
            'Cannot delete the applicant',
          );
        },
      );
  }

  public getFirstTags(applicant: Applicant): Tag[] {
    return applicant.tags.tagDtos.length > 6
      ? applicant.tags.tagDtos.slice(0, 5)
      : applicant.tags.tagDtos;
  }

  public toggleAllTags(): void {
    this.isShowAllTags = this.isShowAllTags ? false : true;
  }

  public applyFilter(): void {
    this.searchValue = this.searchValue;
    this.dataSource.filter = this.searchValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.directive?.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  public clearSearchInput(): void {
    this.dataSource.filter = '';
    this.searchValue = '';

    if (this.dataSource.paginator) {
      this.directive?.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  public sortData(sort: Sort): void {
    this.dataSource.data = (this.dataSource.data as Applicant[]).sort(
      (a, b) => {
        const isAsc = sort.direction === 'asc';

        switch (sort.active) {
          case 'name':
            return this.compareRows(
              a.firstName + ' ' + a.lastName + ' ' + a.middleName,
              b.firstName + ' ' + b.lastName + ' ' + b.middleName,
              isAsc,
            );
          case 'email':
            return this.compareRows(a.email, b.email, isAsc);
          case 'active_vacancies':
            return this.compareRows(
              a.vacancies.length,
              b.vacancies.length,
              isAsc,
            );
          case 'jobs_list':
            return this.compareRows(
              a.vacancies.length,
              b.vacancies.length,
              isAsc,
            );
          case 'tags':
            return this.compareRows(
              a.tags.tagDtos.length,
              b.tags.tagDtos.length,
              isAsc,
            );
          default:
            return 0;
        }
      },
    );
  }

  private compareRows(
    a: number | string,
    b: number | string,
    isAsc: boolean,
  ): number {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
}
