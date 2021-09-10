import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { MainPageComponent } from './components/main-page/main-page.component';
import { TaskCardComponent } from './components/task-card/task-card.component';
import { TaskInfoWidgetComponent } from './components/task-info-widget/task-info-widget.component';
import { AllInOneComponent } from './components/modal/all-in-one/all-in-one.component';
import { MyTasksComponent } from './components/my-tasks/my-tasks.component';



@NgModule({
  declarations: [
    MainPageComponent,
    TaskCardComponent,
    TaskInfoWidgetComponent,
    AllInOneComponent,    
  ],
  imports: [
    CommonModule,
    SharedModule,
  ],
})
export class TaskManagementModule { }
