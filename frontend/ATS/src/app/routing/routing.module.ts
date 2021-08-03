import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserRoutingModule } from '../users/user-routing.module';
import { VacanciesRoutingModule } from '../vacancies/vacancies-routing.module';

const routes: Routes = [];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    UserRoutingModule,
    VacanciesRoutingModule,
  ],
  exports: [RouterModule],
})
export class RoutingModule {}
