import { Observable, Subject } from 'rxjs';
import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { map, takeUntil } from 'rxjs/operators';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { MatPaginator } from '@angular/material/paginator';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { Tag } from 'src/app/shared/models/tags/tag';
import { ViewableApplicant } from 'src/app/shared/models/applicants/viewable-applicant';

@Component({
  selector: 'app-applicants',
  templateUrl: 'applicants.component.html',
  styleUrls: ['applicants.component.scss'],
})
export class ApplicantsComponent implements OnInit, OnDestroy, AfterViewInit {
  public displayedColumns: string[] = [
    'position',
    'name',
    'email',
    'active_vacancies',
    'jobs_list',
    'tags',
    'control_buttons',
  ];

  public dataSource = new MatTableDataSource<ViewableApplicant>();
  public cashedData: ViewableApplicant[] = [];
  public searchValue = '';
  public isFollowedPage = false;
  public loading: boolean = true;

  @ViewChild(MatPaginator) public paginator: MatPaginator | undefined =
  undefined;
  @ViewChild(StylePaginatorDirective) public directive:
  | StylePaginatorDirective
  | undefined = undefined;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly notificationsService: NotificationService,
    private readonly applicantsService: ApplicantsService,
  ) {}

  public ngOnInit(): void {
    this.getApplicants();
  }

  public getApplicants(){
    this.applicantsService
      .getApplicants()
      .pipe(
        takeUntil(this.unsubscribe$),
        map((arr) =>
          arr.map((a) => {
            let viewableApplicant = a as unknown as ViewableApplicant;
            
            viewableApplicant.isFollowed = false;
            viewableApplicant.isShowAllTags = false;

            if (!a.tags) {
              viewableApplicant.tags = {
                id: '',
                elasticType: 1,
                tagDtos: [],
              };
            }

            return viewableApplicant;
          }),
        ),
      )
      .subscribe(
        (result: ViewableApplicant[]) => {
          result.forEach((d, i) => {
            d.position = i + 1;
          });
          
          this.loading = false;
          this.dataSource.data = result;
          this.cashedData = result;
          this.directive!.applyFilter$.emit();
        },
        (error: Error) => {
          this.loading = false;

          this.notificationsService.showErrorMessage(
            error.message,
            'Cannot download applicants from the host',
          );
        },
      );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
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
            viewableApplicant.isFollowed = false;
            viewableApplicant.position = this.cashedData.length + 1;
          }

          return viewableApplicant;
        }),
      )
      .subscribe((result: ViewableApplicant) => {
        if (result) {
          this.cashedData.unshift(result);
          this.dataSource.data = this.cashedData;

          if (this.isFollowedPage) {
            this.dataSource.data = this.dataSource.data.filter(a => a.isFollowed);
          }

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
      applicant.position = this.cashedData[applicantIndex].position;
      applicant.isFollowed = this.cashedData[applicantIndex].isFollowed;
      this.cashedData[applicantIndex] = applicant;
      this.dataSource.data = this.cashedData;

      if (this.isFollowedPage) {
        this.dataSource.data = this.dataSource.data.filter(a => a.isFollowed);
      }
    
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

    if (this.isFollowedPage) {
      this.dataSource.data = this.dataSource.data.filter(a => a.isFollowed);
    }

    this.directive?.applyFilter$.emit();
    this.notificationsService.showSuccessMessage(
      'The applicant was successfully deleted',
      'Success!',
    );
  }

  public toggleFollowedOrAll(isFollowed: boolean): void {
    this.isFollowedPage = isFollowed;

    if (isFollowed) {
      this.dataSource.data = this.dataSource.data.filter(a => a.isFollowed);
    }
    else {
      this.dataSource.data = this.cashedData;
    }

    this.directive!.applyFilter$.emit();
  }

  public onBookmark(applicantId: string){
    const applicantIndex = this.cashedData.findIndex(a => a.id === applicantId);
    this.cashedData[applicantIndex].isFollowed = !this.cashedData[applicantIndex].isFollowed;
    this.dataSource.data = this.cashedData;

    if (this.isFollowedPage) {
      this.dataSource.data = this.dataSource.data.filter(a => a.isFollowed);
    }

    this.directive!.applyFilter$.emit();
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
    applicant.isShowAllTags = applicant.isShowAllTags ? false : true;
  }

  public sortData(sort: Sort): void {
    this.dataSource.data = (this.dataSource.data as ViewableApplicant[]).sort(
      (a, b) => {
        const isAsc = sort.direction === 'asc';

        switch (sort.active) {
          case 'position':
            return this.compareRows(a.position, b.position, isAsc);
          case 'name':
            return this.compareRows(
              a.firstName + ' ' + a.lastName,
              b.firstName + ' ' + b.lastName,
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

  public updateListApplicants(){
    this.getApplicants();
  }

  private compareRows(
    a: number | string,
    b: number | string,
    isAsc: boolean,
  ): number {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
}
