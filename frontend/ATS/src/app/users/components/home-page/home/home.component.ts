import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { HomeWidgetData } from 'src/app/users/models/home/home-widget-data';
import { HotVacancySummary } from 'src/app/users/models/home/hot-vacancy-summary';
import { HomeDataService } from 'src/app/users/services/home-data.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit, OnDestroy {    
  public applicantWidget! : HomeWidgetData;
  public activeJobsWidgetFull! : HomeWidgetData;
  public processedWidget! : HomeWidgetData;
  public recruiterWidget! : HomeWidgetData;

  public hotVacancies: HotVacancySummary[] = [];
  public minVisibleQuantity = 5;
  public isShowedAllVacancies: boolean = false;
  public isWidgetsLoading: boolean = true;
  public isVacanciesLoading: boolean = true;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private homeDataService: HomeDataService,
    private notificationService: NotificationService) { }

  public ngOnInit() : void {
    this.homeDataService.GetWidgetsData()
      .pipe(
        takeUntil(this.unsubscribe$),
        finalize(() => this.isWidgetsLoading = false))
      .subscribe(
        widgetsData =>  {
          this.applicantWidget = {    
            iconName: 'person', 
            count: widgetsData.applicantCount, 
            description:'Applicants', 
          };  
        
          this.activeJobsWidgetFull = {
            iconName: 'business_center', 
            count: widgetsData.vacancies.length, 
            description:'Active jobs', 
            list: widgetsData.vacancies,
          };  
        
          this.processedWidget = {
            iconName: 'verified_user', 
            count: widgetsData.processedCount, 
            description:'Processed', 
          };  

          this.recruiterWidget = {
            iconName: 'monitor', 
            count: widgetsData.hrCount, 
            description:'Recruiter', 
          };  
        },
        () => this.notificationService.showErrorMessage('Failed to load widgets data.'),
      );

    this.homeDataService.GetHotVacanciesSummary()
      .pipe(
        takeUntil(this.unsubscribe$),
        finalize(() => this.isVacanciesLoading = false))
      .subscribe(
        hotVacancies => this.hotVacancies = hotVacancies,
        () => this.notificationService.showErrorMessage('Failed to load vacancies summary.'), 
      );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public showAllVacancies() {
    this.isShowedAllVacancies = true;
  }
}