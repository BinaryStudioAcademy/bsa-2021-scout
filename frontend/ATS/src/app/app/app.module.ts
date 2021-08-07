/* eslint-disable max-len */
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RoutingModule } from '../routing/routing.module';
import { AppComponent } from './components/app/app.component';
import { ToastrModule } from 'ngx-toastr';
import { VacanciesModule } from '../vacancies/vacancies.module';
import { ApplicantsModule } from '../applicants/applicants.module';
import { SharedModule } from '../shared/shared.module';
import { UsersModule } from '../users/users.module';
import { VacancyCardComponent } from '../vacancy/vacancy-card/vacancy-card.component';
import { VacancyWidgetComponent } from '../vacancy/vacancy-widget/vacancy-widget.component';
import { HomeComponent } from '../users/components/home/home.component';
import { SidenavService } from '../shared/services/sidenav.service';
import { MenuComponent } from './components/menu/menu.component';

@NgModule({
  declarations: [
    AppComponent, 
    MenuComponent,
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
    ApplicantsModule,
    VacanciesModule,
    UsersModule,
  ],
  providers: [SidenavService],
  bootstrap: [AppComponent],
  exports: [],
})
export class AppModule {}
