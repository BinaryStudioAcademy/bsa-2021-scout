import { Observable, Subject } from 'rxjs';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { map, takeUntil } from 'rxjs/operators';
import { Sort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { UpdateApplicantComponent } from '../update-applicant/update-applicant.component';
import { MatPaginator } from '@angular/material/paginator';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { Tag } from 'src/app/shared/models/tags/tag';
import { ViewableApplicant } from 'src/app/shared/models/applicant/viewable-applicant';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-applicants',
  templateUrl: 'applicants.component.html',
  styleUrls: ['applicants.component.scss'],
})
export class ApplicantsComponent implements OnInit, AfterViewInit {
  public displayedColumns: string[] = [
    'position',
    'name',
    'rate',
    'email',
    'active_vacancies',
    'jobs_list',
    'status',
    'tags',
    'control_buttons',
  ];

  public dataSource = new MatTableDataSource<ViewableApplicant>();
  public cashedData: ViewableApplicant[] = [];
  public searchValue = '';

  @ViewChild(MatPaginator) public paginator: MatPaginator|undefined = undefined;
  @ViewChild(StylePaginatorDirective) public directive: StylePaginatorDirective|undefined 
  = undefined;


  private $unsubscribe = new Subject();

  constructor(
    private readonly dialog: MatDialog,
    private readonly notificationsService: NotificationService,
    private readonly applicantsService: ApplicantsService,
    private readonly route: ActivatedRoute,
  ) {}

  public ngOnInit(): void {
    this.applicantsService.getApplicants()
      .pipe(
        takeUntil(this.$unsubscribe),
        map(arr => arr.map(a => {
          let viewableApplicant = (a as unknown) as ViewableApplicant;
          viewableApplicant.isShowAllTags = false;

          if (!a.tags) {
            viewableApplicant.tags = {
              id: '',
              elasticType: 1,
              tagDtos: [],
            };
          }
  
          return viewableApplicant;
        })),
      )
      .subscribe((result: ViewableApplicant[]) => {
        this.dataSource.data = result;
        this.cashedData = result;
        this.directive!.applyFilter$.emit();
      },
      (error: Error) => {
        this.notificationsService.showErrorMessage(error.message,
          'Cannot download applicants from the host');
      },
      );
  }

  public applySearchValue(searchValue: string) {
    this.searchValue = searchValue;
    this.dataSource.filter = searchValue;

    if (this.dataSource.paginator) {
      this.directive?.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }
 
  public ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator!;
    this.dataSource.filter = this.searchValue.trim().toLowerCase();
  }

  public createApplicant(createdApplicant: Observable<Applicant>): void {
    createdApplicant
      .pipe(
        map((a: Applicant) => {
          let viewableApplicant = (a as unknown) as ViewableApplicant;
          
          if (viewableApplicant) {
            viewableApplicant.isShowAllTags = false;
          }

          return viewableApplicant;
        }),
      )
      .subscribe((result: ViewableApplicant) => {
        if (result) {
          this.cashedData.unshift(result);
          this.dataSource.data = this.cashedData;
          this.directive?.applyFilter$.emit();
        
          this.notificationsService.showSuccessMessage(
            'An applicant was succesfully created',
            'Success!',
          );
        }
      });
  }

  public updateApplicant(applicant: ViewableApplicant): void {
    if (applicant) {
      let applicantIndex = this.cashedData.findIndex(a => a.id === applicant.id);
      this.cashedData[applicantIndex] = applicant;
      this.dataSource.data = this.cashedData;
    
      this.notificationsService.showSuccessMessage(
        'An applicant was succesfully updated',
        'Success!',
      );
    }
  }

  public deleteApplicant(applicantId: string): void {
    let applicantIndex = this.dataSource.data.findIndex(
      (a) => a.id === applicantId,
    );

    this.cashedData.splice(applicantIndex, 1);
    this.dataSource.data = this.cashedData;
    this.directive?.applyFilter$.emit();
    this.notificationsService.showSuccessMessage(
      'The applicant was successfully deleted',
      'Success!',
    );
  }

  public getFirstTags(applicant: ViewableApplicant): Tag[] {
    if (!applicant.tags?.tagDtos) {
      return [];
    }

    return applicant.tags.tagDtos.length > 6
      ? applicant.tags.tagDtos.slice(0, 5)
      : applicant.tags.tagDtos;
  }

  public toggleTags(applicant: ViewableApplicant): void {
    applicant.isShowAllTags = applicant.isShowAllTags
      ? false
      : true;
  }

  public sortData(sort: Sort): void {
    this.dataSource.data = (this.dataSource.data as ViewableApplicant[]).sort((a, b) => {
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
          return this.compareRows(a.tags.tagDtos.length, b.tags.tagDtos.length, isAsc);
        default:
          return 0;
      }
    });
  }

  private compareRows(
    a: number | string,
    b: number | string,
    isAsc: boolean,
  ): number {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
}
