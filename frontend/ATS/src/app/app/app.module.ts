/* eslint-disable max-len */
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RoutingModule } from '../routing/routing.module';
import { AppComponent } from './components/app/app.component';
import { ToastrModule } from 'ngx-toastr';
import { SharedModule } from '../shared/shared.module';
import { VacanciesModule } from '../vacancies/vacancies.module';
import { UsersModule } from '../users/users.module';
import { VacancyCardComponent } from '../vacancy/vacancy-card/vacancy-card.component';
import { VacancyWidgetComponent } from '../vacancy/vacancy-widget/vacancy-widget.component';
import { HomeComponent } from '../users/components/home/home.component';
import { SidenavService } from '../shared/services/sidenav.service';
import { ProjectsModule } from '../projects/projects.module';
import { ErrorInterceptor } from '../users/helpers/error.interceptor';
import { JwtInterceptor } from '../users/helpers/jwt.interceptor';

@NgModule({
  declarations: [
    AppComponent, 
    VacancyCardComponent,
    VacancyWidgetComponent,
    HomeComponent],
  imports: [
    BrowserModule,
    RoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),    
    SharedModule,
    VacanciesModule,
    UsersModule,
    ProjectsModule,
  ],
  providers: [
    SidenavService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
  exports: [],
})
export class AppModule {}
