import { Component, Input, OnInit } from '@angular/core';
import moment from 'moment';

// This line can't be shorter
// eslint-disable-next-line max-len
import { VacancyCandidateWithApplicant } from 'src/app/shared/models/vacancy-candidates/with-applicant';

@Component({
  selector: 'app-candidate-card',
  templateUrl: './candidate-card.component.html',
  styleUrls: ['./candidate-card.component.scss'],
})
export class CandidateCardComponent implements OnInit {
  @Input() public isDraggable: boolean = false;
  @Input() public candidate!: VacancyCandidateWithApplicant;

  public date?: Date;

  public ngOnInit() {
    this.date =
      this.candidate.thirdContactDate ||
      this.candidate.secondContactDate ||
      this.candidate.firstContactDate;
  }

  public fromNow(date: Date): string {
    return moment(date).fromNow();
  }
}
