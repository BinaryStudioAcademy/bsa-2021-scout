import { Component, Input } from '@angular/core';

// This line can't be shorter
// eslint-disable-next-line max-len
import { VacancyCandidateWithApplicant } from 'src/app/shared/models/vacancy-candidates/with-applicant';

@Component({
  selector: 'app-candidate-column',
  templateUrl: './candidate-column.component.html',
  styleUrls: ['./candidate-column.component.scss'],
})
export class CandidateColumnComponent {
  @Input() public isDraggable: boolean = false;
  @Input() public data: VacancyCandidateWithApplicant[] = [];
  @Input() public title: string = 'Data';
}
