import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface CandidateModalProps {
  nextFullName?: string;
  hasPrevious: boolean;
  vacancyName: string;
  candidateId: string;
  vacancyId: string;
}

@Component({
  selector: 'app-one-candidate-modal',
  templateUrl: './one-candidate-modal.component.html',
  styleUrls: ['./one-candidate-modal.component.scss'],
})
export class OneCandidateModalComponent {
  public constructor(
    @Inject(MAT_DIALOG_DATA) public data: CandidateModalProps,
  ) {}
}
