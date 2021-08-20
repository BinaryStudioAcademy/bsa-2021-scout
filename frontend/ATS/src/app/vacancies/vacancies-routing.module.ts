import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainContentComponent } from '../shared/components/main-content/main-content.component';
import { AuthGuard } from '../users/guards/auth.guard';

// This line can't be shorter
// eslint-disable-next-line max-len
import { VacanciesStagesBoardComponent } from './components/vacancies-stages-board/vacancies-stages-board.component';

const routes: Routes = [{
  path: '',
  component: MainContentComponent,
  canActivate: [AuthGuard],
  children: [
    {
      path: 'candidates/:id',
      pathMatch: 'full',
      component: VacanciesStagesBoardComponent,
    },
  ],
}];

@NgModule({
  exports: [RouterModule],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class VacanciesRoutingModule {}
