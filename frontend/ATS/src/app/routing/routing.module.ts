import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {
  VacanciesListComponent,
} from '../vacancies/components/vacancies-list/vacancies-list.component';

import { UserRoutingModule } from '../users/user-routing.module';
import { ProjectRoutingModule } from '../projects/project-routing.module';
import { VacanciesRoutingModule } from '../vacancies/vacancies-routing.module';
import { AppRoute } from './AppRoute';

import {
  VacanciesTableComponent,
} from '../vacancies/components/vacancies-table/vacancies-table.component';

import { MainContentComponent } from '../shared/components/main-content/main-content.component';
import { HomeComponent } from '../users/components/home-page/home/home.component';
import { AuthGuard } from '../users/guards/auth.guard';
import { ApplicantsComponent } from '../applicants/components/applicants/applicants.component';
import { ProjectsListComponent } from 
  '../projects/components/projects-list/projects-list.component';
import { VacanciesStagesBoardComponent } 
  from '../vacancies/components/vacancies-stages-board/vacancies-stages-board.component';
import { UsersTableComponent } from '../users/components/hr-lead/users-table/users-table.component';
import { HrLeadGuard } from '../users/guards/hr-lead.guard';

import {
  ApplicationPoolComponent,
} from '../pools/components/application-pool/application-pool.component';
import { PoolsRoutingModule } from '../pools/pools-routing.module';
import { MailTemplatesListComponent } from 
  '../mail-templates/components/mail-templates-list/mail-templates-list.component';
/* eslint-disable max-len */
import { ApplicantFileTablesComponent } from '../applicants/components/CsvComponents/applicant-csv-file-tables/applicant-csv-file-tables.component';
import { ApplicantsRoutingModule } from '../applicants/applicants-routing.module';
import { UserProfileComponent } from '../shared/components/user-profile/user-profile.component';
import { InterviewsPageComponent } from '../interviews/components/interviews-page/interviews-page.component';
/* eslint-disable max-len */
import { ApplicantCsvFilesListComponent } from '../applicants/components/CsvComponents/applicant-csv-files-list/applicant-csv-files-list.component';

import { ProjectArchiveComponent } from '../archive/components/project-archive/project-archive.component';
import { VacancyArchiveComponent } from '../archive/components/vacancy-archive/vacancy-archive.component';

import { TaskManagementModule } from '../task-management/task-management.module';
import { MainPageComponent } from '../task-management/components/main-page/main-page.component';

const routes: Routes = [
  {
    path: '',
    component: MainContentComponent,
    canActivate: [AuthGuard],
    children: [
      { path: AppRoute.Templates, component: MailTemplatesListComponent, pathMatch: 'full' },
      { 
        path: AppRoute.Home, 
        component: HomeComponent, 
        pathMatch: 'full', 
      },
      {
        path: 'candidates/:id',
        pathMatch: 'full',
        component: VacanciesStagesBoardComponent,
        canActivate: [AuthGuard],
      },
      {
        path: AppRoute.Vacancies,
        component: VacanciesTableComponent,
        pathMatch: 'full',
      },
      {
        path: AppRoute.Applicants,
        component: ApplicantsComponent,
        pathMatch: 'full',
      },
      {
        path: AppRoute.Projects,
        component: ProjectsListComponent,
        pathMatch: 'full',
      },
      {
        path: AppRoute.Interviews,
        component: InterviewsPageComponent,
        pathMatch: 'full',
      },
      {
        path: AppRoute.Archive,
        redirectTo: `${AppRoute.Archive}/${AppRoute.Projects}`,
        pathMatch: 'full',
      },
      {
        path: `${AppRoute.Archive}/${AppRoute.Vacancies}`,
        component: VacancyArchiveComponent,
        pathMatch: 'full',
      },
      {
        path: `${AppRoute.Archive}/${AppRoute.Projects}`,
        component: ProjectArchiveComponent,
        pathMatch: 'full',
      },
      {
        path: AppRoute.TaskManagement,
        component: MainPageComponent,
        pathMatch: 'full',
      },
      {
        path: AppRoute.Templates,
        component: VacanciesListComponent,
        pathMatch: 'full',
      },
      {
        path: AppRoute.Pools,
        component: ApplicationPoolComponent,
        pathMatch: 'full',
      },
      {
        path: AppRoute.Pools,
        component: ApplicationPoolComponent,
        pathMatch: 'full',
      },
      {
        path: AppRoute.UserManagement,
        component: UsersTableComponent,
        canActivate: [HrLeadGuard],
        pathMatch: 'full',
      },
      {
        path: AppRoute.ApplicantsCsv,
        component: ApplicantFileTablesComponent,
        pathMatch: 'full',
      },
      {
        path: AppRoute.ApplicantsCsvFilesList,
        component: ApplicantCsvFilesListComponent,
        pathMatch: 'full',
      },
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
    PoolsRoutingModule,
    ApplicantsRoutingModule,
    TaskManagementModule,
  ],
  exports: [RouterModule],
})
export class RoutingModule {}
