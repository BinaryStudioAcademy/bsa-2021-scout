import { NgModule } from '@angular/core';
import { HeaderModule } from '../header/header.module';
import { VacanciesListComponent } from './components/vacancies-list/vacancies-list.component';
@NgModule({
    declarations: [
      VacanciesListComponent
    ],
    imports:[
      HeaderModule
    ],exports:[
      VacanciesListComponent
    ]
  })
  export class VacanciesModule {}
