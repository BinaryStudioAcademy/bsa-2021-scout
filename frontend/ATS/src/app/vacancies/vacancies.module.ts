import { MatSortModule } from '@angular/material/sort';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
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

import { VacanciesTableComponent } from './components/vacancies-table/vacancies-table.component';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
// This line can't be shorter
// eslint-disable-next-line max-len
import { VacanciesStagesBoardComponent } from './components/vacancies-stages-board/vacancies-stages-board.component';
// This line can't be shorter
// eslint-disable-next-line max-len
import { RateCandidateModalComponent } from './components/rate-candidate-modal/rate-candidate-modal.component';

import { EditVacancyComponent } from './components/edit-vacancy/edit-vacancy.component';
import { CreateStageComponent } from './components/stages/create-stage/create-stage.component';
import { StageComponent } from './components/stages/stage/stage.component';
import { SelectTemplateComponent } from './components/select-template/select-template.component';

@NgModule({
  declarations: [
    CandidateCardComponent,
    CandidateColumnComponent,
    VacanciesStagesBoardComponent,
    VacanciesListComponent,
    OneCandidateComponent,
    OneCandidateModalComponent,
    VacanciesTableComponent,
    RateCandidateModalComponent,
    EditVacancyComponent,
    CreateStageComponent,
    StageComponent,
    SelectTemplateComponent,
  ],
  imports: [
    MatDialogModule,
    CommonModule,
    RoutingModule,
    MatIconModule,
    MatSortModule,
    MatButtonModule,
    MatPaginatorModule,
    MatTableModule,
    MatTabsModule,
    MatInputModule,
    MatFormFieldModule,
    DragDropModule,
    SharedModule,
    CommonModule,
    RoutingModule,
    DragDropModule,
    SharedModule,
  ],
  exports: [
    CandidateCardComponent,
    CandidateColumnComponent,
    VacanciesStagesBoardComponent,
    VacanciesListComponent,
    VacanciesTableComponent,
    OneCandidateComponent,
    OneCandidateModalComponent,
    RateCandidateModalComponent,
  ],
})
export class VacanciesModule {}
