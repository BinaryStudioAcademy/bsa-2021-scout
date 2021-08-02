import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserRoutingModule } from '../users/user-routing.module';

// This line can't be shorter
// eslint-disable-next-line max-len
import { VacanciesStagesBoardComponent } from '../vacancies/components/vacancies-stages-board/vacancies-stages-board.component';

const routes: Routes = [
  {
    path: 'vacancy/:id/stages',
    pathMatch: 'full',
    component: VacanciesStagesBoardComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes), UserRoutingModule],
  exports: [RouterModule],
})
export class RoutingModule {}
