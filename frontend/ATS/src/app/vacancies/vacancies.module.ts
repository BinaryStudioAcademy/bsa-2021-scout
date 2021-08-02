import { NgModule } from '@angular/core';
import { HeaderModule } from '../header/header.module';
import { SharedModule } from '../shared/shared.module';
import { VacanciesListComponent } from './components/vacancies-list/vacancies-list.component';
@NgModule({
    declarations: [
      VacanciesListComponent
    ],
    imports:[
      HeaderModule,
      SharedModule
    ],exports:[
      VacanciesListComponent
    ]
  })
  export class VacanciesModule {}
