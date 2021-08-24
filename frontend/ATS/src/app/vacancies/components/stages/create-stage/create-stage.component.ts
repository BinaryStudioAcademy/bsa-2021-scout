import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Action } from 'src/app/shared/models/action/action';
import { ActionType } from 'src/app/shared/models/action/action-type';
import { Stage } from 'src/app/shared/models/stages/stage';
import { StageType } from 'src/app/shared/models/stages/type';

@Component({
  selector: 'app-create-stage',
  templateUrl: './create-stage.component.html',
  styleUrls: ['./create-stage.component.scss'],
})
export class CreateStageComponent implements OnChanges {

  submitted: Boolean = false;
  editModeItemIndex: number = -1;
  @Output() stageChange = new EventEmitter<Stage>();
  @Output() stageCreateAndAddChange = new EventEmitter<Stage>();
  @Output() isClosedChange = new EventEmitter<Boolean>();
  @Input() stage: Stage = {} as Stage;
  stageId: string = '';
  constructor(private fb: FormBuilder) {
    this.stageForm = this.fb.group({
      name: ['', [Validators.required]],
      type: ['', [Validators.required]],
      actions: ['', [Validators.required]],
      isReviewable: [''],
      rates: ['', [Validators.required]],
    },
    );
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.stage && this.stageForm) {
      if(changes.stage.currentValue.actions){
        var finalArray = changes.stage.currentValue.actions.map(function (obj: Action) {
          return obj.actionType;
        });
        changes.stage.currentValue.actions.forEach((element: Action) => {
          var index = this.actions.findIndex(x => x.actionType == element.actionType);
          this.actions[index] = element;
        });
        this.stageForm.get('actions')?.setValue(finalArray);
      }
      this.stageId = changes.stage.currentValue.id;
      this.stageForm.get('name')?.setValue(changes.stage.currentValue.name);
      this.stageForm.get('type')?.setValue(changes.stage.currentValue.type);
      this.stageForm.get('isReviewable')?.setValue(changes.stage.currentValue.isReviewable);
      this.stageForm.get('rates')?.setValue(changes.stage.currentValue.rates);
      this.editModeItemIndex = changes.stage.currentValue.index;
    }
  }

  save() {
    this.submitted = true;
    this.stage = this.stageForm.value;
    this.stage.index = this.editModeItemIndex;
    this.stageForm.reset();
  }

  types : {name: string, type: StageType }[] = [
    { name:'Applied', type: StageType.Applied },
    { name:'Phone screen', type: StageType.PhoneScreen },
    { name:'Interview', type: StageType.Interview },
    { name:'Test', type: StageType.Test },
    { name:'Offer', type: StageType.Offer },
    { name:'Hired', type: StageType.Hired },
  ]

  actions: Action[] = [
    { id: undefined, name: 'None', actionType: ActionType.None, stageId: this.stageId },
    { id: undefined, name: 'Send mail', actionType: ActionType.SendMail, stageId: this.stageId },
    { id: undefined, name: 'Add task', actionType: ActionType.AddTask, stageId: this.stageId },
    {
      id: undefined, name: 'Schedule interview action',
      actionType: ActionType.ScheduleInterviewAction,
      stageId: this.stageId,
    },
  ];

  onStageSave() {
    this.submitted = true;
    this.stage = this.stageForm.value;
    this.stage.index = this.editModeItemIndex;
    this.stage.actions = [];
    this.stageForm.get('actions')?.value.forEach((element: number) => {
      this.stage.actions.push(this.actions.find(x => x.actionType == element) as Action);
    });
    this.stageForm.reset();
    this.stageChange.emit(this.stage);
    console.log(this.stage);
    this.stage = {} as Stage;
  
  }

  onSaveAndAdd() {
    this.submitted = true;
    this.stage = this.stageForm.value;
    this.stage.index = this.editModeItemIndex;
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