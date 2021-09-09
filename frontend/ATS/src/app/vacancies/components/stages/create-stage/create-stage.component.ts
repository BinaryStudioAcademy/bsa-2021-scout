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
import { of, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { IOption } from 'src/app/shared/components/multiselect/multiselect.component';
import { Action } from 'src/app/shared/models/action/action';
import { ActionType } from 'src/app/shared/models/action/action-type';
import { Review } from 'src/app/shared/models/reviews/review';
import { Stage } from 'src/app/shared/models/stages/stage';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ReviewService } from 'src/app/shared/services/review.service';
import { StageType } from 'src/app/shared/models/stages/type';
import { MatDialog } from '@angular/material/dialog';
import { SelectTemplateComponent } from '../../select-template/select-template.component';

interface SendEmailDataJson{
  joinTemplateId: string;
  leaveTemplateId: string;
}

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
  public actionJoinOptions: IOption[] = [];
  public actionLeaveOptions: IOption[] = [];
  public dataJson : SendEmailDataJson = {
    joinTemplateId: '',
    leaveTemplateId: '',
  };

  private static reviews: Review[] = [];
  private static reviewsLoaded: boolean = false;

  private actionsOnJoin: Action[] = [
    {
      id: '1',
      name: 'Send mail',
      actionType: ActionType.SendMail,
      stageId: this.stageId,
      stageChangeEventType: 1,
    },
    {
      id: '2',
      name: 'Add task',
      actionType: ActionType.AddTask,
      stageId: this.stageId,
      stageChangeEventType: 1,
    },
    {
      id: '3',
      name: 'Schedule interview action',
      actionType: ActionType.ScheduleInterviewAction,
      stageId: this.stageId,
      stageChangeEventType: 1,
    },
  ];

  private actionsOnLeave: Action[] = [
    {
      id: '1',
      name: 'Send mail',
      actionType: ActionType.SendMail,
      stageId: this.stageId,
      stageChangeEventType: 0,
    },
  ];

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly fb: FormBuilder,
    private readonly reviewService: ReviewService,
    private readonly notifications: NotificationService,
    private readonly dialog: MatDialog,
  ) {
    this.stageForm = this.fb.group({
      name: ['', [Validators.required]],
      type: ['', [Validators.required]],
      actionsOnJoin: [[]],
      actionsOnLeave: [[]],
      isReviewable: [false],
      reviews: [[]],
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.stage && this.stageForm) {
      if (changes.stage.currentValue.actions) {
        changes.stage.currentValue.actions.forEach((element: Action) => {
          if(element.stageChangeEventType == 1){
            var index = this.actionsOnJoin.findIndex(
              (x) => x.actionType == element.actionType,
            );
            this.actionsOnJoin[index] = element;
          }
          else{
            var index = this.actionsOnLeave.findIndex(
              (x) => x.actionType == element.actionType,
            );
            this.actionsOnLeave[index] = element;
          }
        });
        let actions: Action[] = changes.stage.currentValue.actions;
        let actionsOnJoin: Action[] = [];
        let actionsOnLeave: Action[] = [];
        actions.forEach(action => {
          if(action.stageChangeEventType == 0)
          {
            actionsOnLeave.push(action);
          }
          else
          {
            actionsOnJoin.push(action);
          }
        });
        this.stageForm
          .get('actionsOnJoin')
          ?.setValue([...actionsOnJoin]);
        this.stageForm
          .get('actionsOnLeave')
          ?.setValue([...actionsOnLeave]);
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
    if(this.stage?.dataJson != '' && this.stage?.dataJson != null 
    && this.stage?.dataJson != undefined){
      this.dataJson = JSON.parse(this.stage?.dataJson ? this.stage!.dataJson : '');
    }
    
    if (CreateStageComponent.reviewsLoaded) {
      this.reviewOptions = this.reviewsToOptions(CreateStageComponent.reviews);
    } else {
      this.loadReviews();
    }

    if (this.stage) {
      this.actionJoinOptions = this.actionsToOptions(this.actionsOnJoin);
      this.actionLeaveOptions = this.actionsToOptions(this.actionsOnLeave);
      if (this.stage?.id != null) {
        this.editing = true;
      } else {
        this.stage = {} as Stage;
      }
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

  selectedChange(options?: IOption[], stageChangeEventType: number = 0){
    let newActions: Action[] = this.optionsToActions(options, stageChangeEventType);
    let actions: Action[] = [];
    if(stageChangeEventType == 1){
      actions = this.stageForm.get('actionsOnJoin')!.value;
    }
    else{
      actions = this.stageForm.get('actionsOnLeave')!.value;
    }
    let isExist : boolean = false;
    let isNewExist : boolean = false;
    actions.forEach(action => {
      if(action.name == 'Send mail')
      {
        isExist = true;
      }
    });

    newActions.forEach(action => {
      if(action.name == 'Send mail')
      {
        isNewExist = true;
      }
    });

    if (!isExist && isNewExist)
    {
      this.dialog.open(SelectTemplateComponent, { width: '600px', disableClose: true })
        .afterClosed()
        .subscribe(result => {
          let sendMail: SendEmailDataJson = {
            joinTemplateId: '',
            leaveTemplateId: '',
          };
          if(stageChangeEventType == 1){
            sendMail = {
              joinTemplateId: result,
              leaveTemplateId: this.dataJson.leaveTemplateId,
            };
          }
          else{
            sendMail = {
              joinTemplateId: this.dataJson.joinTemplateId,
              leaveTemplateId: result,
            };
          }

          this.dataJson = sendMail;
        });
    }

    if(stageChangeEventType == 1){
      this.stageForm.get('actionsOnJoin')!.setValue(newActions);
    }
    else{
      this.stageForm.get('actionsOnLeave')!.setValue(newActions);
    }
  }
  
  public actionsToOptions(actions?: Action[]): IOption[] {
    return (actions ?? []).map((a) => ({
      id: a.id,
      value: a.id,
      label: a.name,
    }));
  }

  public optionsToActions(options?: IOption[], stageChangeEventType: number = 0): Action[] {
    if(stageChangeEventType == 1){
      return (options ?? []).map(
        ({ id }) => this.actionsOnJoin.find((a) => a.id === id)!,
      );
    }
    else{
      return (options ?? []).map(
        ({ id }) => this.actionsOnLeave.find((a) => a.id === id)!,
      );
    }
  }

  formToStage() {
    let actionsOnJoin: Action[] = this.stageForm.get('actionsOnJoin')?.value;
    actionsOnJoin.forEach(action => action.stageChangeEventType = 1);
    let actionsOnLeave: Action[] = this.stageForm.get('actionsOnLeave')?.value;
    actionsOnLeave.forEach(action => action.stageChangeEventType = 0);
    let actions: Action[] = [...actionsOnJoin, ...actionsOnLeave];

    let stage: Stage = {
      id: this.stage?.id ? this.stage!.id : '',
      name: this.stageForm.get('name')?.value,
      type: this.stageForm.get('type')?.value,
      IsReviewable: this.stageForm.get('isReviewable')?.value,
      reviews: this.stageForm.get('reviews')?.value,
      index: 0,
      vacancyId: '',
      actions: actions,
      dataJson: JSON.stringify(this.dataJson),
    };
    
    this.stage = stage;
  }

  save() {
    this.submitted = true;
    this.formToStage();
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
    this.formToStage();
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
    this.formToStage();
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
