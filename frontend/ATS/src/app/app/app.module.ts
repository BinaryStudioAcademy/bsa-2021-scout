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
import { VacancyCardComponent } from '../vacancies/components/vacancy-card/vacancy-card.component';
import { VacancyWidgetComponent } from '../vacancies/components/vacancy-widget/vacancy-widget.component';
import { HomeComponent } from '../users/components/home/home.component';
import { SidenavService } from '../shared/services/sidenav.service';


import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';

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
    MatSortModule,
    MatTableModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),    
    SharedModule,
    VacanciesModule,
    UsersModule,
  ],
  providers: [SidenavService, 
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
  exports: [],
})
export class AppModule {}
