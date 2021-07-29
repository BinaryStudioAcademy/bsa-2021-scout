import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ShortCandidate } from 'src/app/shared/models/candidates/short';
import { CandidateStatus } from 'src/app/shared/models/candidates/status';
import { CandidateService } from 'src/app/shared/services/candidate.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

enum Filter {
  Qualified,
  Disqualified,
  Sourced,
}

@Component({
  selector: 'app-vacancies-stages-board',
  templateUrl: './vacancies-stages-board.component.html',
  styleUrls: ['./vacancies-stages-board.component.scss'],
})
export class VacanciesStagesBoardComponent implements OnInit, OnDestroy {
  public loading: boolean = true;
  public data: ShortCandidate[] = [];
  public filter: Filter = Filter.Qualified;

  public cachedApplied: ShortCandidate[] = [];
  public cachedPhoneScreen: ShortCandidate[] = [];
  public cachedInterview: ShortCandidate[] = [];
  public cachedTest: ShortCandidate[] = [];
  public cachedOffer: ShortCandidate[] = [];
  public cachedHired: ShortCandidate[] = [];

  public applied: ShortCandidate[] = [];
  public phoneScreen: ShortCandidate[] = [];
  public interview: ShortCandidate[] = [];
  public test: ShortCandidate[] = [];
  public offer: ShortCandidate[] = [];
  public hired: ShortCandidate[] = [];

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly candidateService: CandidateService,
    private readonly notificationService: NotificationService,
  ) {}

  public ngOnInit(): void {
    this.loadData();
    this.classifyData();
    this.filterData();
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public filterData(): void {
    this.applied = this.filterDataArray(this.cachedApplied);
    this.phoneScreen = this.filterDataArray(this.cachedPhoneScreen);
    this.interview = this.filterDataArray(this.cachedInterview);
    this.test = this.filterDataArray(this.cachedTest);
    this.offer = this.filterDataArray(this.cachedOffer);
    this.hired = this.filterDataArray(this.cachedHired);
  }

  public setFilter(index: number): void {
    switch (index) {
      case 1:
        this.filter = Filter.Disqualified;
        break;
      case 2:
        this.filter = Filter.Sourced;
        break;
      case 0:
      default:
        this.filter = Filter.Qualified;
        break;
    }
  }

  private loadData(): void {
    this.candidateService
      .getShortCandidates()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (data) => {
          this.data = data;
          this.loading = false;
        },
        () => {
          this.notificationService.showErrorMessage(
            'Failed to load data',
            'Error',
          );
        },
      );
  }

  private classifyData(): void {
    this.data.forEach((candidate) => {
      switch (candidate.status) {
        case CandidateStatus.Applied:
          this.cachedApplied.push(candidate);
          break;
        case CandidateStatus.PhoneScreen:
          this.cachedPhoneScreen.push(candidate);
          break;
        case CandidateStatus.Interview:
          this.cachedInterview.push(candidate);
          break;
        case CandidateStatus.Test:
          this.cachedTest.push(candidate);
          break;
        case CandidateStatus.Offer:
          this.cachedOffer.push(candidate);
          break;
        case CandidateStatus.Hired:
          this.cachedHired.push(candidate);
          break;
      }
    });
  }

  private filterDataArray(array: ShortCandidate[]): ShortCandidate[] {
    return array; //
  }
}
