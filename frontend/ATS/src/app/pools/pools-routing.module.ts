import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../users/guards/auth.guard';
import { ApplicationPoolDetailsComponent } 
  from './components/application-pool-details/application-pool-details.component';

const routes: Routes = [
  {
    path: 'pools/:id',
    pathMatch: 'full',
    component: ApplicationPoolDetailsComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  exports: [RouterModule],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class PoolsRoutingModule {}
