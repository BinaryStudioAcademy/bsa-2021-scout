import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FullVacancyCandidate } from 'src/app/shared/models/vacancy-candidates/full';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { VacancyCandidateService } from 'src/app/shared/services/vacancy-candidate.service';

@Component({
  selector: 'app-one-candidate',
  templateUrl: './one-candidate.component.html',
  styleUrls: ['./one-candidate.component.scss'],
})
export class OneCandidateComponent implements OnInit, OnDestroy {
  @Input() public id!: string;

  public data!: FullVacancyCandidate;
  public loading: boolean = true;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly service: VacancyCandidateService,
    private readonly notificationService: NotificationService,
  ) {}

  public ngOnInit() {
    this.loadData(this.id);
  }

  public ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  private loadData(id: string) {
    this.service.getFull(id).subscribe(
      (data) => {
        this.loading = false;
        this.data = data;
      },
      () => {
        this.notificationService.showErrorMessage('Failed to load', 'Error');
      },
    );
  }
}
