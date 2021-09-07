import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
} from '@angular/cdk/drag-drop';

import { EMPTY, forkJoin, Observable, Subject } from 'rxjs';
import { mergeMap, takeUntil } from 'rxjs/operators';
import { StageService } from 'src/app/shared/services/stage.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { StageWithCandidates } from 'src/app/shared/models/stages/with-candidates';
import { VacancyCandidateService } from 'src/app/shared/services/vacancy-candidate.service';

// This line can't be shorter
// eslint-disable-next-line max-len
import { ShortVacancyCandidateWithApplicant } from 'src/app/shared/models/vacancy-candidates/short-with-applicant';
import { OneCandidateModalComponent } from '../one-candidate-modal/one-candidate-modal.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';

// This line can't be shorter
// eslint-disable-next-line max-len
import { RateCandidateModalComponent } from '../rate-candidate-modal/rate-candidate-modal.component';
import { ShortVacancyWithStages } from 'src/app/shared/models/vacancy/short-with-stages';
import { ReviewService } from 'src/app/shared/services/review.service';
import { Review } from 'src/app/shared/models/reviews/review';

// This line can't be shorter
// eslint-disable-next-line max-len
import { AddCandidateModalComponent } from 'src/app/shared/components/modal-add-candidate/modal-add-candidate.component';
import { ArchivationService } from 'src/app/archive/services/archivation.service';
import { AppRoute } from 'src/app/routing/AppRoute';
import { ConfirmationDialogComponent } 
  from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';

interface CandidatePos {
  index: number;
  stageIndex: number;
}

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
  public vacancyId: string = '';

  private reviews: Review[] = [];

  private readonly showingAvatarsCount: number = 7;
  private readonly additionalCriteriaMap: Record<string, Review[]> = {};
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly stageService: StageService,
    private readonly vacancyCandidateService: VacancyCandidateService,
    private readonly reviewService: ReviewService,
    private readonly notificationService: NotificationService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly modalService: MatDialog,
    private readonly archivationService: ArchivationService,
  ) {}

  public ngOnInit(): void {
    this.route.params.subscribe(({ id }) => {
      this.loading = true;
      this.vacancyId = id;
      this.loadData();
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
      const forward = () =>
        transferArrayItem(
          event.previousContainer.data,
          event.container.data,
          event.previousIndex,
          event.currentIndex,
        );

      const backward = () =>
        transferArrayItem(
          event.container.data,
          event.previousContainer.data,
          event.currentIndex,
          event.previousIndex,
        );

      const updateStage = () =>
        this.vacancyCandidateService
          .changeCandidateStage(
            event.item.element.nativeElement.id, // Stores candidate id
            this.vacancyId,
            event.container.id, // Stores new stage id
          )
          .subscribe(
            (_) =>
              this.MarkCandidateAsViewed(event.item.element.nativeElement.id),
            (_) => {
              this.notificationService.showErrorMessage(
                'Failed to save candidate\'s stage',
                'Error',
              );

              backward();
            },
          );

      const stage = this.data.find(
        (s) => s.id === event.container.id,
      ) as StageWithCandidates;

      forward();

      if (stage.index == 0) {
        return backward();
      }

      if (stage.isReviewable) {
        const dialog: MatDialogRef<RateCandidateModalComponent> =
          this.modalService.open(RateCandidateModalComponent, {
            width: '700px',
            data: {
              fixedCriterias: stage.reviews,
              optionalCriterias: this.additionalCriteriaMap[stage.id],
              stageId: stage.id,
              candidateId: event.item.element.nativeElement.id,
            },
          });

        dialog.afterClosed().subscribe((result) => {
          if (result === 'none' || !result) {
            return backward();
          }

          updateStage();
        });
      } else {
        updateStage();
      }
    }
  }

  public openCandidateAddModal(): void {
    this.modalService
      .open(AddCandidateModalComponent, {
        width: '500px',
        autoFocus: false,
        panelClass: 'candidate-dialog',
        data: {
          vacancyId: this.vacancyId,
        },
      })
      .afterClosed()
      .subscribe((_) => this.loadData());
  }

  public openCompletingVacancyConfirmDialog(): void {
    this.modalService.open(ConfirmationDialogComponent, {
      width: '450px',
      height: 'min-content',
      autoFocus: false,
      data: {
        action: 'Complete',
        entityName: 'vacancy',
        additionalMessage: 'Vacancy will be archived.',
      },
    })
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap(response => {
          if (response) {
            return this.archivationService.archiveVacancy(this.vacancyId, true);
          }
          return EMPTY;
        }),
      )
      .subscribe(
        (_) => {
          this.notificationService.showSuccessMessage(
            `Vacancy ${this.title} closed!`,
          );
          this.router.navigate([AppRoute.Vacancies]);
        },
        () => {
          this.notificationService.showErrorMessage(
            'Vacancy closing is failed!',
          );
        },
      );
  }

  public openCandidateModal(id: string): void {
    let pos: CandidatePos = { index: 0, stageIndex: 0 };
    let prevPos: CandidatePos | undefined;
    let hasPrevious: boolean = false;
    let nextPos: CandidatePos | undefined;
    let nextFullName: string | undefined;

    this.MarkCandidateAsViewed(id);

    this.data.forEach((stage, sIndex) =>
      stage.candidates.forEach((candidate, index) => {
        if (candidate.id === id) {
          pos.stageIndex = sIndex;
          pos.index = index;
        }
      }),
    );

    const lastStage = this.data.length - 1;
    const lastCandidate = this.data[pos.stageIndex].candidates.length - 1;

    if (pos.stageIndex > 0 || pos.index > 0) {
      if (pos.index > 0) {
        prevPos = { index: pos.index - 1, stageIndex: pos.stageIndex };
        hasPrevious = true;
      } else {
        let currentStage: number = pos.stageIndex;

        while (true) {
          currentStage -= 1;

          if (currentStage === -1) {
            break;
          }

          if (this.data[currentStage].candidates.length > 0) {
            hasPrevious = true;

            prevPos = {
              index: this.data[currentStage].candidates.length - 1,
              stageIndex: currentStage,
            };

            break;
          }
        }
      }
    }

    if (pos.stageIndex < lastStage || pos.index < lastCandidate) {
      if (pos.index < lastCandidate) {
        nextPos = { index: pos.index + 1, stageIndex: pos.stageIndex };

        const nextApplicant =
          this.data[nextPos.stageIndex].candidates[nextPos.index].applicant;

        nextFullName = nextApplicant.firstName + ' ' + nextApplicant.lastName;
      } else {
        let currentStage: number = pos.stageIndex;

        while (true) {
          currentStage += 1;

          if (currentStage === lastStage + 1) {
            break;
          }

          if (this.data[currentStage].candidates.length > 0) {
            nextPos = {
              index: 0,
              stageIndex: currentStage,
            };

            const nextApplicant =
              this.data[nextPos.stageIndex].candidates[nextPos.index].applicant;

            nextFullName =
              nextApplicant.firstName + ' ' + nextApplicant.lastName;

            break;
          }
        }
      }
    }

    this.modalService
      .open(OneCandidateModalComponent, {
        maxWidth: '700px',
        data: {
          vacancyName: this.title,
          candidateId: id,
          vacancyId: this.vacancyId,
          hasPrevious,
          nextFullName,
        },
      })
      .afterClosed()
      .subscribe((state) => {
        if (state === 'prev') {
          this.openCandidateModal(
            this.data[prevPos!.stageIndex].candidates[prevPos!.index].id,
          );
        } else if (state === 'next') {
          this.openCandidateModal(
            this.data[nextPos!.stageIndex].candidates[nextPos!.index].id,
          );
        }
      });
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

  private loadData(): void {
    const stages$: Observable<ShortVacancyWithStages> =
      this.stageService.getByVacancyIdWithCandidates(this.vacancyId);

    const reviews$: Observable<Review[]> = this.reviewService.getAll();

    forkJoin({
      reviews: reviews$,
      vacancy: stages$,
    }).subscribe(
      ({ reviews, vacancy }) => {
        this.loading = false;

        this.data = [...vacancy.stages];
        vacancy.stages.forEach((stage,index) =>{
          if(stage.index==0 && stage.candidates.length == 0){
            vacancy.stages.splice(index, 1);
            this.data = [...vacancy.stages];
          }
        });

        this.reviews = [...reviews];
        this.title = vacancy.title;

        this.filterData();
        this.prepareLists();
        this.prepareAvatars();
        this.prepareAdditionalCriterias();
      },
      () => {
        this.loading = false;

        this.notificationService.showErrorMessage(
          'Failed to load data',
          'Error',
        );
      },
    );

    stages$.pipe(takeUntil(this.unsubscribe$)).subscribe();
  }

  private prepareLists(): void {
    this.listIds = this.data.map(({ id }) => id);
  }

  private prepareAvatars(): void {
    const allAvatars: string[] = [];
    let totalCandidates: number = 0;

    this.data.forEach((stage) =>
      stage.candidates.forEach(() => {
        totalCandidates += 1;
      }),
    );

    this.data.forEach((stage) =>
      stage.candidates.forEach(
        (_candidate) =>
          allAvatars.push('../../../../assets/images/defaultAvatar.png'), //
      ),
    );

    this.avatars = allAvatars.slice(0, this.showingAvatarsCount);
    this.extraAvatarsCount = totalCandidates - this.avatars.length;
  }

  private prepareAdditionalCriterias(): void {
    this.data.forEach((stage) => {
      const fixedIds = stage.reviews.map((r) => r.id);
      const additional = this.reviews.filter((r) => !fixedIds.includes(r.id));

      this.additionalCriteriaMap[stage.id] = [...additional];
    });
  }

  public MarkCandidateAsViewed(id: string) {
    this.data.forEach((stage) => {
      stage.candidates.forEach((candidate) => {
        if (candidate.id == id) {
          if (candidate.isViewed != true) {
            this.vacancyCandidateService.MarkAsViewed(id).subscribe((_) => {
              candidate.isViewed = true;
            });
          }
        }
      });
    });
  }
}
