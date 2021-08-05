import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OneCandidateComponent } from './components/one-candidate/one-candidate.component';

// This line can't be shorter
// eslint-disable-next-line max-len
import { VacanciesStagesBoardComponent } from './components/vacancies-stages-board/vacancies-stages-board.component';

const routes: Routes = [
  {
    path: 'candidates/:id',
    pathMatch: 'full',
    component: VacanciesStagesBoardComponent,
  },
  {
    path: 'candidate/:id',
    pathMatch: 'full',
    component: OneCandidateComponent,
  },
];

@NgModule({
  exports: [RouterModule],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class VacanciesRoutingModule {}
