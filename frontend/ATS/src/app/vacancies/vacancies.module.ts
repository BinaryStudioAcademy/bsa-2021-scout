import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { RoutingModule } from '../routing/routing.module';
import { SharedModule } from '../shared/shared.module';
import { CandidateCardComponent } from './components/candidate-card/candidate-card.component';
import { CandidateColumnComponent } from './components/candidate-column/candidate-column.component';

// This line can't be shorter
// eslint-disable-next-line max-len
import { VacanciesStagesBoardComponent } from './components/vacancies-stages-board/vacancies-stages-board.component';

import { VacanciesListComponent } from './components/vacancies-list/vacancies-list.component';
import { VacanciesTableComponent } from './components/vacancies-table/vacancies-table.component';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import {MatPaginatorModule} from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
@NgModule({
  declarations: [
    CandidateCardComponent,
    CandidateColumnComponent,
    VacanciesStagesBoardComponent,
    VacanciesListComponent,
    VacanciesTableComponent,
    StylePaginatorDirective,
  ],
  imports: [
    MatDialogModule,
    CommonModule,
    RoutingModule,
    MatIconModule,
    MatButtonModule,
    MatPaginatorModule,
    MatTableModule,
    MatTabsModule,
    DragDropModule,
    SharedModule,
  ],
  exports: [
    CandidateCardComponent,
    CandidateColumnComponent,
    VacanciesStagesBoardComponent,
    VacanciesListComponent,
    VacanciesTableComponent,
  ],
})
export class VacanciesModule {}
