import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { IOption } from 'src/app/shared/components/multiselect/multiselect.component';
import { Action } from 'src/app/shared/models/action/action';
import { ActionType } from 'src/app/shared/models/action/action-type';
import { Review } from 'src/app/shared/models/reviews/review';
import { Stage } from 'src/app/shared/models/stages/stage';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ReviewService } from 'src/app/shared/services/review.service';
import { StageType } from 'src/app/shared/models/stages/type';

@Component({
  selector: 'app-create-stage',
  templateUrl: './create-stage.component.html',
  styleUrls: ['./create-stage.component.scss'],
})
export class CreateStageComponent implements OnChanges, OnInit, OnDestroy {
  @Output() stageChange = new EventEmitter<Stage>();
  @Output() stageCreateAndAddChange = new EventEmitter<Stage>();
  @Output() isClosedChange = new EventEmitter<boolean>();
  @Input() stage?: Stage;

  public submitted: Boolean = false;
  public editModeItemIndex: number = -1;
  public loading: boolean = false;
  public stageId: string = '';
  public editing: boolean = false;
  public reviewOptions: IOption[] = [];
  public actionOptions: IOption[] = [];

  private static reviews: Review[] = [];
  private static reviewsLoaded: boolean = false;

  private actions: Action[] = [
    {
      id: '1',
      name: 'Send mail',
      actionType: ActionType.SendMail,
      stageId: this.stageId,
    },
    {
      id: '2',
      name: 'Add task',
      actionType: ActionType.AddTask,
      stageId: this.stageId,
    },
    {
      id: '3',
      name: 'Schedule interview action',
      actionType: ActionType.ScheduleInterviewAction,
      stageId: this.stageId,
    },
  ];

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly fb: FormBuilder,
    private readonly reviewService: ReviewService,
    private readonly notifications: NotificationService,
  ) {
    this.stageForm = this.fb.group({
      name: ['', [Validators.required]],
      type: ['', [Validators.required]],
      actions: [[]],
      isReviewable: [false],
      reviews: [[]],
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.stage && this.stageForm) {
      if (changes.stage.currentValue.actions) {
        changes.stage.currentValue.actions.forEach((element: Action) => {
          var index = this.actions.findIndex(
            (x) => x.actionType == element.actionType,
          );
          this.actions[index] = element;
        });
        this.stageForm
          .get('actions')
          ?.setValue([...changes.stage.currentValue.actions]);
      }
      this.stageId = changes.stage.currentValue.id;
      this.stageForm.get('name')?.setValue(changes.stage.currentValue.name);
      this.stageForm.get('type')?.setValue(changes.stage.currentValue.type);
      this.stageForm
        .get('isReviewable')
        ?.setValue(changes.stage.currentValue.isReviewable);
      this.stageForm
        .get('reviews')
        ?.setValue(changes.stage.currentValue.reviews);
      this.editModeItemIndex = changes.stage.currentValue.index ?? -1;
    }
  }

  public ngOnInit(): void {
    if (CreateStageComponent.reviewsLoaded) {
      this.reviewOptions = this.reviewsToOptions(CreateStageComponent.reviews);
    } else {
      this.loadReviews();
    }

    this.actionOptions = this.actionsToOptions(this.actions);
    if (this.stage?.id != null) {
      this.editing = true;
    } else {
      this.stage = {} as Stage;
    }
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public loadReviews(): void {
    this.loading = true;

    this.reviewService
      .getAll()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (reviews) => {
          this.loading = false;
          this.reviewOptions = this.reviewsToOptions(reviews);
          CreateStageComponent.reviews = [...reviews];
        },
        () => {
          this.loading = false;
          this.notifications.showErrorMessage('Failed to load reviews');
        },
      );
  }

  public reviewsToOptions(reviews?: Review[]): IOption[] {
    return (reviews ?? []).map((r) => ({
      id: r.id,
      value: r.id,
      label: r.name,
    }));
  }

  public optionsToReviews(options?: IOption[]): Review[] {
    return (options ?? []).map(
      ({ id }) => CreateStageComponent.reviews.find((r) => r.id === id)!,
    );
  }

  public actionsToOptions(actions?: Action[]): IOption[] {
    return (actions ?? []).map((a) => ({
      id: a.id,
      value: a.id,
      label: a.name,
    }));
  }

  public optionsToActions(options?: IOption[]): Action[] {
    return (options ?? []).map(
      ({ id }) => this.actions.find((a) => a.id === id)!,
    );
  }

  save() {
    this.submitted = true;
    this.stage = { ...this.stageForm.value };
    this.stage!.index = this.editModeItemIndex;
    this.stageForm.reset();
  }

  types: { name: string; type: StageType }[] = [
    { name: 'Applied', type: StageType.Applied },
    { name: 'Phone screen', type: StageType.PhoneScreen },
    { name: 'Interview', type: StageType.Interview },
    { name: 'Offer', type: StageType.Offer },
    { name: 'Hired', type: StageType.Hired },
  ];

  onStageSave() {
    this.submitted = true;
    this.stage = { ...this.stageForm.value };
    if(this.stage!.reviews == null){
      this.stage!.reviews = [];
    }
    this.stage!.index = this.editModeItemIndex;
    this.stageForm.reset();
    this.stageChange.emit(this.stage);
    this.stage = {} as Stage;
  }

  onSaveAndAdd() {
    this.submitted = true;
    this.stage = this.stageForm.value;
    this.stage!.index = this.editModeItemIndex;
    this.stageForm.reset();
    this.stageCreateAndAddChange.emit(this.stage);
    this.stage = {} as Stage;
  }

  onStageClose() {
    this.submitted = false;
    this.stageForm.reset();
    this.isClosedChange.emit(true);
    this.stage = {} as Stage;
  }

  get stageFormControl() {
    return this.stageForm.controls;
  }

  stageForm!: FormGroup;
}
