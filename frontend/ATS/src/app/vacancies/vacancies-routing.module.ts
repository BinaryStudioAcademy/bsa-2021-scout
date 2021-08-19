import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../users/guards/auth.guard';

// This line can't be shorter
// eslint-disable-next-line max-len
import { VacanciesStagesBoardComponent } from './components/vacancies-stages-board/vacancies-stages-board.component';

const routes: Routes = [
  {
    path: 'candidates/:id',
    pathMatch: 'full',
    component: VacanciesStagesBoardComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  exports: [RouterModule],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class VacanciesRoutingModule {}
