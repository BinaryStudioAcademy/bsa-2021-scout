import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VacancyWithRecentActivity }
  from 'src/app/shared/models/candidate-to-stages/vacancy-with-recent-activity';

@Component({
  selector: 'app-applicant-history',
  templateUrl: './applicant-history.component.html',
  styleUrls: ['./applicant-history.component.scss'],
})
export class ApplicantHistoryComponent {
  public constructor(@Inject(MAT_DIALOG_DATA) public data: VacancyWithRecentActivity[]) {}
}
