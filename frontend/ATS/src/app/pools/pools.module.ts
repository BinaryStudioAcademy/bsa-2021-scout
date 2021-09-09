import { NgModule, LOCALE_ID } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
// eslint-disable-next-line
import { ApplicationPoolDetailsComponent } from './components/application-pool-details/application-pool-details.component';
import { ApplicationPoolComponent } from './components/application-pool/application-pool.component';
// eslint-disable-next-line
import { CreateTalentpoolModalComponent } from './components/create-talentpool-modal/create-talentpool-modal.component';
// eslint-disable-next-line
import { EditAppPoolModalComponent } from './components/edit-app-pool-modal/edit-app-pool-modal.component';
// eslint-disable-next-line
import { PoolDetailsModalComponent } from './components/pool-details-modal/pool-details-modal.component';
import { SelectModalComponent } from './components/modal/select-modal/select-modal.component';

@NgModule({
  declarations: [
    ApplicationPoolComponent,
    CreateTalentpoolModalComponent,
    EditAppPoolModalComponent,
    ApplicationPoolDetailsComponent,
    PoolDetailsModalComponent,
    SelectModalComponent,
  ],
  imports: [SharedModule, RouterModule],
  providers: [],
})
export class PoolsModule {}
