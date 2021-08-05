import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

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
import { VacancyCandidateService } from 'src/app/shared/services/vacancy-candidate.service';

// This line can't be shorter
// eslint-disable-next-line max-len
import { ShortVacancyCandidateWithApplicant } from 'src/app/shared/models/vacancy-candidates/short-with-applicant';

@Component({
  selector: 'app-vacancies-stages-board',
  templateUrl: './vacancies-stages-board.component.html',
  styleUrls: ['./vacancies-stages-board.component.scss'],
})
export class VacanciesStagesBoardComponent implements OnInit, OnDestroy {
  public loading: boolean = true;
  public data: StageWithCandidates[] = [];
  public title: string = '';
  public listIds: string[] = [];
  public avatars: string[] = [];
  public extraAvatarsCount: number = 0;

  private readonly showingAvatarsCount: number = 7;
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly stageService: StageService,
    private readonly vacationCandidateService: VacancyCandidateService,
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

  public onMove(event: CdkDragDrop<ShortVacancyCandidateWithApplicant[]>) {
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

      this.vacationCandidateService
        .changeCandidateStage(
          event.item.element.nativeElement.id, // Stores candidate id
          event.container.id, // Stores new stage id
        )
        .subscribe(
          () =>
            this.notificationService.showSuccessMessage(
              'Candidate\'s stage is updated',
              'Success',
            ),
          () => {
            this.notificationService.showErrorMessage(
              'Failed to save candidate\'s stage',
              'Error',
            );

            transferArrayItem(
              event.container.data,
              event.previousContainer.data,
              event.currentIndex,
              event.previousIndex,
            );
          },
        );
    }
  }

  private loadData(vacancyId: string): void {
    this.stageService
      .getByVacancyIdWithCandidates(vacancyId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (data) => {
          this.data = data.stages;
          this.title = data.title;
          this.loading = false;

          this.filterData();
          this.prepareLists();
          this.prepareAvatars();
        },
        () => {
          this.notificationService.showErrorMessage(
            'Failed to load data',
            'Error',
          );
        },
      );
  }

  public getLinkedWith(id: string): string[] {
    const index: number = this.listIds.findIndex((lid) => lid === id);
    const prevIndex: number = Math.max(index - 1, 0);
    const nextIndex: number = Math.min(index + 1, this.listIds.length - 1);

    const indexes: number[] = [prevIndex, index, nextIndex];

    const distinctIndexes: number[] = [];

    indexes.forEach((i) => {
      if (!distinctIndexes.includes(i)) {
        distinctIndexes.push(i);
      }
    });

    return indexes.map((i) => this.listIds[i]);
  }

  private prepareLists(): void {
    this.listIds = this.data.map(({ id }) => id);
  }

  private prepareAvatars(): void {
    const allAvatars: string[] = [];

    this.data.forEach((stage) =>
      stage.candidates.forEach(
        (_candidate) =>
          allAvatars.push('../../../../assets/images/defaultAvatar.png'), //
      ),
    );

    this.avatars = allAvatars.slice(0, this.showingAvatarsCount);
    this.extraAvatarsCount = this.data.length - this.avatars.length + 1;
  }
}
