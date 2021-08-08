import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { RoutingModule } from '../routing/routing.module';
import { SharedModule } from '../shared/shared.module';
import { CandidateCardComponent } from './components/candidate-card/candidate-card.component';
import { CandidateColumnComponent } from './components/candidate-column/candidate-column.component';
import { VacanciesListComponent } from './components/vacancies-list/vacancies-list.component';
import { OneCandidateComponent } from './components/one-candidate/one-candidate.component';

// This line can't be shorter
// eslint-disable-next-line max-len
import { OneCandidateModalComponent } from './components/one-candidate-modal/one-candidate-modal.component';

// This line can't be shorter
// eslint-disable-next-line max-len
import { VacanciesStagesBoardComponent } from './components/vacancies-stages-board/vacancies-stages-board.component';

@NgModule({
  declarations: [
    CandidateCardComponent,
    CandidateColumnComponent,
    VacanciesStagesBoardComponent,
    VacanciesListComponent,
    OneCandidateComponent,
    OneCandidateModalComponent,
  ],
  imports: [CommonModule, RoutingModule, DragDropModule, SharedModule],
  exports: [
    CandidateCardComponent,
    CandidateColumnComponent,
    VacanciesStagesBoardComponent,
    VacanciesListComponent,
    OneCandidateComponent,
    OneCandidateModalComponent,
  ],
})
export class VacanciesModule {}
