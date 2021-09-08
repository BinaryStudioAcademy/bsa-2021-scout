import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { ProjectArchiveComponent } from './components/project-archive/project-archive.component';
import { VacancyArchiveComponent } from './components/vacancy-archive/vacancy-archive.component';
import { ArchiveDataService } from './services/archive-data.service';
import { UnarchiveProjectDialogComponent } 
  from './components/unarchive-project-dialog/unarchive-project-dialog.component';

@NgModule({
  declarations: [
    ProjectArchiveComponent,
    VacancyArchiveComponent,
    UnarchiveProjectDialogComponent,
  ],
  imports:[
    CommonModule,
    SharedModule,
  ],
  exports:[

  ],
  providers: [
    ArchiveDataService,
  ],
})
export class ArchiveModule {}
