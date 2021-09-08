import { InterviewsModule } from './../interviews/interviews.module';
/* eslint-disable max-len */
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RoutingModule } from '../routing/routing.module';
import { AppComponent } from './components/app/app.component';
import { ToastrModule } from 'ngx-toastr';
import { VacanciesModule } from '../vacancies/vacancies.module';
import { ApplicantsModule } from '../applicants/applicants.module';
import { SharedModule } from '../shared/shared.module';
import { UsersModule } from '../users/users.module';
import { SidenavService } from '../shared/services/sidenav.service';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { ErrorInterceptor } from '../users/helpers/error.interceptor';
import { JwtInterceptor } from '../users/helpers/jwt.interceptor';
import { AuthGuard } from '../users/guards/auth.guard';
import { HrLeadGuard } from '../users/guards/hr-lead.guard';

import { ProjectsModule } from '../projects/projects.module';
import { PoolsModule } from '../pools/pools.module';
import { ArchiveModule } from '../archive/archive.module';
import { MailTemplatesModule } from '../mail-templates/mail-templates.module';
import { TaskManagementModule } from '../task-management/task-management.module';


@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    RoutingModule,
    HttpClientModule,
    MatSortModule,
    MatTableModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    SharedModule,
    ApplicantsModule,
    VacanciesModule,
    InterviewsModule,
    UsersModule,
    ProjectsModule,
    PoolsModule,
    ArchiveModule,
    MailTemplatesModule,
    TaskManagementModule,
  ],
  providers: [
    SidenavService,
    AuthGuard,
    HrLeadGuard,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
  exports: [],
})
export class AppModule {}
