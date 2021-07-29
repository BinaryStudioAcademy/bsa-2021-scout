import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VacanciesListComponent } from '../vacancies/components/vacancies-list/vacancies-list.component';

const routes: Routes = [
  { path: 'home', component: VacanciesListComponent, pathMatch: 'full' },
  { path: 'vacancies', component: VacanciesListComponent, pathMatch: 'full' },
  { path: 'applicants', component: VacanciesListComponent, pathMatch: 'full' },
  { path: 'interviews', component: VacanciesListComponent, pathMatch: 'full' },
  { path: 'analytics', component: VacanciesListComponent, pathMatch: 'full' },
  { path: 'taskManagement', component: VacanciesListComponent, pathMatch: 'full' },
  { path: 'templates', component: VacanciesListComponent, pathMatch: 'full' },
  { path: '**', redirectTo: 'home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class RoutingModule {}
