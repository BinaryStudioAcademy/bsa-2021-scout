import { Routes } from '@angular/router';

// This line can't be shorter
// eslint-disable-next-line max-len
import { VacanciesStagesBoardComponent } from './components/vacancies-stages-board/vacancies-stages-board.component';

export const vacanciesRoutes: Routes = [
  {
    path: ':id/stages',
    pathMatch: 'full',
    component: VacanciesStagesBoardComponent,
  },
];
