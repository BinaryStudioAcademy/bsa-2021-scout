import { Component, OnDestroy, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { IOption } from 'src/app/shared/components/multiselect/multiselect.component';
import { Review } from 'src/app/shared/models/reviews/review';
import { CandidateReviewService } from 'src/app/shared/services/candidate-review.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

interface Props {
  fixedCriterias: Review[];
  optionalCriterias: Review[];
  stageId: string;
  candidateId: string;
}

@Component({
  selector: 'app-rate-candidate-modal',
  templateUrl: './rate-candidate-modal.component.html',
  styleUrls: ['./rate-candidate-modal.component.scss'],
})
export class RateCandidateModalComponent implements OnInit, OnDestroy {
  public rateData: Record<string, number> = {};
  public optionalCriteriaOptions: IOption[] = [];
  public selectedOptionalCriteriaOptions: IOption[] = [];
  public selectedOptionalCriterias: Review[] = [];
  public comment: string = '';

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly candidateReviewService: CandidateReviewService,
    private readonly notifications: NotificationService,
    private readonly dialogRef: MatDialogRef<RateCandidateModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Props,
  ) {}

  public ngOnInit(): void {
    this.optionalCriteriaOptions = this.data.optionalCriterias.map((crit) => ({
      id: crit.id,
      value: crit.id,
      label: crit.name,
    }));
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public onChangeOptionalCriterias(newCriteriaOptions: IOption[]): void {
    this.selectedOptionalCriteriaOptions = [...newCriteriaOptions];

    this.selectedOptionalCriterias = newCriteriaOptions.map((opt) => ({
      id: opt.id as string,
      name: opt.label,
    }));
  }

  public isButtonDisabled(): boolean {
    return this.data.fixedCriterias.some(
      (crit) => typeof this.rateData[crit.id] !== 'number',
    );
  }

  public submit(): void {
    console.log(this.rateData);
    this.candidateReviewService
      .bulkReview(
        this.data.stageId,
        this.data.candidateId,
        this.comment || undefined,
        this.rateData,
      )
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        () => this.dialogRef.close('rate'),
        () => this.notifications.showErrorMessage('Failed to rate'),
      );
  }
}
