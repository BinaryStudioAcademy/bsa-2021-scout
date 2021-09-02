import { Component, EventEmitter, Input, Output } from '@angular/core';
// eslint-disable-next-line max-len
import { ShortVacancyCandidateWithApplicant } from 'src/app/shared/models/vacancy-candidates/short-with-applicant';

@Component({
  selector: 'app-candidate-card',
  templateUrl: './candidate-card.component.html',
  styleUrls: ['./candidate-card.component.scss'],
})
export class CandidateCardComponent {
  @Input() public isDraggable: boolean = false;
  @Input() public isClickable: boolean = false;
  @Input() public candidate!: ShortVacancyCandidateWithApplicant;
  @Output() public clickAction: EventEmitter<void> = new EventEmitter<void>();

  public emitClick(): void {
    if (this.isClickable) {
      this.clickAction.emit();
    }
  }
}
