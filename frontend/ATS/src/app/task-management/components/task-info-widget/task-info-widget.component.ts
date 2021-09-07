import { Component, Input } from '@angular/core';
import {widgetInfo} from '../../models/widget-info';

@Component({
  selector: 'app-task-info-widget',
  templateUrl: './task-info-widget.component.html',
  styleUrls: ['./task-info-widget.component.scss'],
})

export class TaskInfoWidgetComponent {
  @Input() public info!: widgetInfo;

  constructor() { }

}
