import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-vacancy-widget',
  templateUrl: './vacancy-widget.component.html',
  styleUrls: ['./vacancy-widget.component.scss'],
})
export class VacancyWidgetComponent{
  @Input() widgetInfo : any;
  
}
