import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { MainPageComponent } from './components/main-page/main-page.component';
import { TaskCardComponent } from './components/task-card/task-card.component';



@NgModule({
  declarations: [
    MainPageComponent,
    TaskCardComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
  ],
})
export class TaskManagementModule { }
