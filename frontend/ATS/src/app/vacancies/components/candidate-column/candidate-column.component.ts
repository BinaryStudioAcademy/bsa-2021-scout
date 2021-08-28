import { Component, EventEmitter, Input, Output } from '@angular/core';

// This line can't be shorter
// eslint-disable-next-line max-len
import { ShortVacancyCandidateWithApplicant } from 'src/app/shared/models/vacancy-candidates/short-with-applicant';

@Component({
  selector: 'app-candidate-column',
  templateUrl: './candidate-column.component.html',
  styleUrls: ['./candidate-column.component.scss'],
})
export class CandidateColumnComponent {
  @Input() public isDraggable: boolean = false;
  @Input() public data: ShortVacancyCandidateWithApplicant[] = [];
  @Input() public title: string = 'Data';
  @Input() public index: number = -1;
  @Output() public clickAction: EventEmitter<string> =
  new EventEmitter<string>();

  public emitClick(id: string): void {
    this.clickAction.emit(id);
  }
}
