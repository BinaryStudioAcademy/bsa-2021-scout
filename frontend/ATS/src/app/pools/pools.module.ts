import { NgModule, LOCALE_ID } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { ApplicationPoolDetailsComponent } 
  from './components/application-pool-details/application-pool-details.component';
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
    ApplicationPoolDetailsComponent,
  ],
  imports: [SharedModule,RouterModule],
  providers: [
  ],
})
export class PoolsModule { }
