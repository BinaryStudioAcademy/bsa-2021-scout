import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserRoutingModule } from '../users/user-routing.module';
import { vacanciesRoutes } from '../vacancies/vacancies.routing';

const routes: Routes = [
  {
    path: 'vacancies',
    children: vacanciesRoutes,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes), UserRoutingModule],
  exports: [RouterModule],
})
export class RoutingModule {}
