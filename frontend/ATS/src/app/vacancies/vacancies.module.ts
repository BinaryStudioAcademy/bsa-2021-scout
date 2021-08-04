import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { VacanciesListComponent } from './components/vacancies-list/vacancies-list.component';
@NgModule({
  declarations: [
    VacanciesListComponent,
  ],
  imports:[
    SharedModule,
  ],exports:[
    VacanciesListComponent,
  ],
})
export class VacanciesModule {}
