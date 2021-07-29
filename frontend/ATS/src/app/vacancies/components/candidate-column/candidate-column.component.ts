import { Component, Input } from '@angular/core';
import { ShortCandidate } from 'src/app/shared/models/candidates/short';

@Component({
  selector: 'app-candidate-column',
  templateUrl: './candidate-column.component.html',
  styleUrls: ['./candidate-column.component.scss'],
})
export class CandidateColumnComponent {
  @Input() public isDraggable: boolean = false;
  @Input() public data: ShortCandidate[] = [];
  @Input() public title: string = 'Data';
}
