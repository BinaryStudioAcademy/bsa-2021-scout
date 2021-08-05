import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import{VacanciesListComponent}from'../vacancies/components/vacancies-list/vacancies-list.component';
import { UserRoutingModule } from '../users/user-routing.module';
import { VacanciesRoutingModule } from '../vacancies/vacancies-routing.module';
import { AppRoute } from './AppRoute';
import { VacanciesTableComponent }
  from '../vacancies/components/vacancies-table/vacancies-table.component';
 
const routes: Routes = [
  { path: AppRoute.Home, component: VacanciesListComponent, pathMatch: 'full' },
  { path: AppRoute.Vacancies, component: VacanciesTableComponent, pathMatch: 'full' },
  { path: AppRoute.Applicants, component: VacanciesListComponent, pathMatch: 'full' },
  { path: AppRoute.Interviews, component: VacanciesListComponent, pathMatch: 'full' },
  { path: AppRoute.Analytics, component: VacanciesListComponent, pathMatch: 'full' },
  { path: AppRoute.TaskManagement, component: VacanciesListComponent, pathMatch: 'full' },
  { path: AppRoute.Templates, component: VacanciesListComponent, pathMatch: 'full' },
  { path: '**', redirectTo: AppRoute.Home },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    UserRoutingModule,
    VacanciesRoutingModule,
  ],
  exports: [RouterModule],
})
export class RoutingModule {}
