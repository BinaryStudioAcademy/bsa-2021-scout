import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { RecentActivity } from 'src/app/shared/models/candidate-to-stages/recent-activity';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { RecentActivityService } from 'src/app/shared/services/recent-activity.service';

@Component({
  selector: 'app-recent-activity',
  templateUrl: './recent-activity.component.html',
  styleUrls: ['./recent-activity.component.scss'],
})
export class RecentActivityComponent implements OnInit, OnDestroy {
  public data: RecentActivity[] = [];
  public isEnd: boolean = false;
  public loading: boolean = false;

  private page: number = 1;
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly service: RecentActivityService,
    private readonly notifications: NotificationService,
  ) {}

  public ngOnInit(): void {
    this.loadData();
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public loadMoreData(): void {
    this.page++;
    this.loadData(this.page);
  }

  private loadData(page: number = 1): void {
    this.loading = true;

    this.service
      .getRecentActivity(page)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (result) => {
          this.loading = false;
          this.data = [...this.data, ...result.data];
          this.isEnd = result.isEnd;
        },
        () => {
          this.loading = false;

          this.notifications.showErrorMessage(
            'Failed to load recent activity.',
          );
        },
      );
  }
}
