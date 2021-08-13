import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VacanciesListComponent } from
  '../vacancies/components/vacancies-list/vacancies-list.component';
import { UserRoutingModule } from '../users/user-routing.module';
import { ProjectRoutingModule } from '../projects/project-routing.module';
import { VacanciesRoutingModule } from '../vacancies/vacancies-routing.module';
import { AppRoute } from './AppRoute';
import { MainContentComponent } from
  '../shared/components/main-content/main-content.component';
import { HomeComponent } from '../users/components/home/home.component';
import { AuthGuard } from '../users/guards/auth.guard';
import { ApplicantsComponent } from '../applicants/components/applicants/applicants.component';
import { ProjectsListComponent } from 
  '../projects/components/projects-list/projects-list.component';

const routes: Routes = [
  {
    path: '', component: MainContentComponent, canActivate:[AuthGuard], children: [
      { path: AppRoute.Home, component: HomeComponent, pathMatch: 'full' },
      { path: AppRoute.Vacancies, component: VacanciesListComponent, pathMatch: 'full' },
      { path: AppRoute.Applicants, component: ApplicantsComponent, pathMatch: 'full' },
      { path: AppRoute.Projects, component: ProjectsListComponent, pathMatch: 'full' },
      { path: AppRoute.Interviews, component: VacanciesListComponent, pathMatch: 'full' },
      { path: AppRoute.Analytics, component: VacanciesListComponent, pathMatch: 'full' },
      { path: AppRoute.TaskManagement, component: VacanciesListComponent, pathMatch: 'full' },
      { path: AppRoute.Templates, component: VacanciesListComponent, pathMatch: 'full' },
      { path: '**', redirectTo: AppRoute.Home },
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    UserRoutingModule,
    VacanciesRoutingModule,
    ProjectRoutingModule,
  ],
  exports: [RouterModule],
})
export class RoutingModule { }
