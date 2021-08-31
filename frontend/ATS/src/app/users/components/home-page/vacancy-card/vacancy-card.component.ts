import { Component, Input } from '@angular/core';
import { HotVacancySummary } from 'src/app/users/models/home/hot-vacancy-summary';

@Component({
  selector: 'app-vacancy-card',
  templateUrl: './vacancy-card.component.html',
  styleUrls: ['./vacancy-card.component.scss'],
})
export class VacancyCardComponent {
  @Input() hotVacancy!: HotVacancySummary;
  @Input() isBlur: boolean = false;
}
