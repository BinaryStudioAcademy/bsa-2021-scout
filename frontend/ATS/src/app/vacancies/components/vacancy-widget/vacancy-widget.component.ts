import { Component, Input } from '@angular/core';
import { VacancyWidgetInfo } from 'src/app/shared/models/vacancy/vacancy-widget';

@Component({
  selector: 'app-vacancy-widget',
  templateUrl: './vacancy-widget.component.html',
  styleUrls: ['./vacancy-widget.component.scss'],
})
export class VacancyWidgetComponent {
  @Input() public widgetInfo!: VacancyWidgetInfo;
}
