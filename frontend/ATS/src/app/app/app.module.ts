import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RoutingModule } from '../routing/routing.module';
import { AppComponent } from './components/app/app.component';
import { ToastrModule } from 'ngx-toastr';
import { MatSidenavModule } from '@angular/material/sidenav';
import {MatIconModule} from '@angular/material/icon';
import { SharedModule } from '../shared/shared.module';
import { HeaderModule } from '../header/header.module';
import { MainPageElementsModule } from '../main-page-elements/main-page-elements.module';
import { VacanciesModule } from '../vacancies/vacancies.module';
import { UsersModule } from '../users/users.module';
import { SidenavService } from '../shared/services/sidenav.service';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    RoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,  
    ToastrModule.forRoot(),
    MainPageElementsModule,
    MatSidenavModule,
    MatIconModule,
    HeaderModule,
    SharedModule,
    VacanciesModule,
    UsersModule,
  ],
  providers: [SidenavService],
  bootstrap: [AppComponent],
  exports: [],
})
export class AppModule {}
