import { NgModule, LOCALE_ID } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { ApplicationPoolComponent } 
  from './components/application-pool/application-pool.component';
import { CreateTalentpoolModalComponent } 
  from './components/create-talentpool-modal/create-talentpool-modal.component';
import { EditAppPoolModalComponent } 
  from './components/edit-app-pool-modal/edit-app-pool-modal.component';


@NgModule({
  declarations: [
    ApplicationPoolComponent,
    CreateTalentpoolModalComponent,
    EditAppPoolModalComponent,    
  ],
  imports: [SharedModule],
  providers: [
  ],
})
export class PoolsModule { }
