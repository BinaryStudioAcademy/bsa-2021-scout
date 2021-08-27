import { Component, Input } from '@angular/core';
import { HomeWidgetData } from 'src/app/users/models/home/home-widget-data';

@Component({
  selector: 'app-home-widget',
  templateUrl: './home-widget.component.html',
  styleUrls: ['./home-widget.component.scss'],
})
export class HomeWidgetComponent {
  @Input() widgetData?: HomeWidgetData;
}
