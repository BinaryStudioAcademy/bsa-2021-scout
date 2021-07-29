import { Component, Input } from '@angular/core';
import moment from 'moment';
import { ShortCandidate } from 'src/app/shared/models/candidates/short';

@Component({
  selector: 'app-candidate-card',
  templateUrl: './candidate-card.component.html',
  styleUrls: ['./candidate-card.component.scss'],
})
export class CandidateCardComponent {
  @Input() public isDraggable: boolean = false;
  @Input() public card!: ShortCandidate;

  public fromNow(date: Date): string {
    return moment(date).fromNow();
  }
}
