import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Stage } from 'src/app/shared/models/stages/stage';

@Component({
  selector: 'app-stage',
  templateUrl: './stage.component.html',
  styleUrls: ['./stage.component.scss'],
})
export class StageComponent {

  constructor() { }
  @Input() stage: Stage = {} as Stage;
  @Output() isDeletedStage = new EventEmitter<Stage>();
  @Output() isEditStage = new EventEmitter<Stage>();


  deleteStage(){
    this.isDeletedStage.emit(this.stage);
  }

  editStage(){
    this.isEditStage.emit(this.stage);
  }

}