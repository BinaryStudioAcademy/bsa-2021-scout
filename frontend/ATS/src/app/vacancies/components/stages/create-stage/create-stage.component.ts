import {Component,EventEmitter,Input,OnChanges,OnInit,Output,SimpleChanges} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Stage } from 'src/app/shared/models/stages/stage';

@Component({
  selector: 'app-create-stage',
  templateUrl: './create-stage.component.html',
  styleUrls: ['./create-stage.component.scss'],
})
export class CreateStageComponent implements OnChanges {

  submitted:Boolean = false;
  editModeItemIndex:number=-1;
  @Output() stageChange = new EventEmitter<Stage>();
  @Output() stageCreateAndAddChange = new EventEmitter<Stage>();
  @Output() isClosedChange = new EventEmitter<Boolean>();
  @Input() stage:Stage = {} as Stage;

  constructor(private fb: FormBuilder) {
    this.stageForm = this.fb.group({
      name: ['', [Validators.required]],
      action: ['', [Validators.required]],
      isReviewRequired: [''],
      rates: ['', [Validators.required]],
    },
    );
  }

  ngOnChanges(changes: SimpleChanges){
    if(changes.stage && this.stageForm){
      this.stageForm.get('name')?.setValue(changes.stage.currentValue.name);
      this.stageForm.get('action')?.setValue(changes.stage.currentValue.action);
      this.stageForm.get('isReviewRequired')?.setValue(changes.stage.currentValue.isReviewRequired);
      this.stageForm.get('rates')?.setValue(changes.stage.currentValue.rates);
      this.editModeItemIndex = changes.stage.currentValue.index;
    }
  }

  save(){
    this.submitted = true;
    this.stage = this.stageForm.value;
    this.stage.index = this.editModeItemIndex;
    this.stageForm.reset();
  }

  onStageSave(){
    this.submitted = true;
    this.stage = this.stageForm.value;
    this.stage.index = this.editModeItemIndex;
    this.stageForm.reset();
    this.stageChange.emit(this.stage);
    this.stage = {} as Stage;
  }

  onSaveAndAdd(){
    this.submitted = true;
    this.stage = this.stageForm.value;
    this.stage.index = this.editModeItemIndex;

    this.stageForm.reset();
    this.stageCreateAndAddChange.emit(this.stage);

    this.stage = {} as Stage;
  }

  onStageClose(){
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