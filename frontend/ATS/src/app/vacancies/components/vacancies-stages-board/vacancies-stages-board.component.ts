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
import { OneCandidateModalComponent } from '../one-candidate-modal/one-candidate-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { AddCandidateModalComponent }
  from 'src/app/shared/components/modal-add-candidate/modal-add-candidate.component';

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

  private readonly showingAvatarsCount: number = 7;
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly stageService: StageService,
    private readonly vacancyCandidateService: VacancyCandidateService,
    private readonly notificationService: NotificationService,
    private readonly route: ActivatedRoute,
    private readonly modalService: MatDialog,
  ) { }

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

      this.vacancyCandidateService
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

  public openCandidateAddModal(): void {
    this.modalService.open(AddCandidateModalComponent, {
      width: '400px',
      autoFocus: false,
      panelClass: 'candidate-dialog',
      data: {
        vacancyId: this.vacancyId,
      },
    }).afterClosed()
      .subscribe(_ => this.loadData(this.vacancyId));
  }


  public openCandidateModal(id: string): void {
    let pos: CandidatePos = { index: 0, stageIndex: 0 };
    let prevPos: CandidatePos | undefined;
    let hasPrevious: boolean = false;
    let nextPos: CandidatePos | undefined;
    let nextFullName: string | undefined;

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
        } else if (state == 'next') {
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

  private loadData(vacancyId: string): void {
    this.stageService
      .getByVacancyIdWithCandidates(vacancyId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (data) => {
          this.data = data.stages;
          this.title = data.title;
          this.vacancyId = vacancyId;
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
}
