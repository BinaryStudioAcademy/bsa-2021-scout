import { Component, OnInit, OnDestroy } from '@angular/core';

import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
} from '@angular/cdk/drag-drop';

import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { StageService } from 'src/app/shared/services/stage.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { StageWithCandidates } from 'src/app/shared/models/stages/with-candidates';
import { ActivatedRoute } from '@angular/router';

// This line can't be shorter
// eslint-disable-next-line max-len
import { VacancyCandidateWithApplicant } from 'src/app/shared/models/vacancy-candidates/with-applicant';

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
  public data: StageWithCandidates[] = [];
  public filter: Filter = Filter.Qualified;
  public listIds: string[] = [];

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly stageService: StageService,
    private readonly notificationService: NotificationService,
    private readonly route: ActivatedRoute,
  ) {}

  public ngOnInit(): void {
    this.route.params.subscribe(({ id }) => {
      this.loading = true;
      this.loadData(id);
    });
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public filterData(): void {
    //
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

  public onMove(event: CdkDragDrop<VacancyCandidateWithApplicant[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
    }
  }

  private loadData(vacancyId: string): void {
    this.stageService
      .getByVacancyIdWithCandidates(vacancyId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (data) => {
          this.data = data;
          this.loading = false;

          this.filterData();
          this.prepareLists();
        },
        () => {
          this.notificationService.showErrorMessage(
            'Failed to load data',
            'Error',
          );
        },
      );
  }

  private prepareLists(): void {
    this.listIds = this.data.map(({ id }) => `list-${id}`);
  }

  public l(a: any) {
    console.log(a);
    return 'Abc';
  }
}
