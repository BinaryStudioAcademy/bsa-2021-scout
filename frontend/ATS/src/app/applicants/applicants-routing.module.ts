import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SelfApplyingComponent } from './components/self-applying/self-applying.component';

const routes: Routes = [
  {
    path: 'vacancy/apply/:id',
    pathMatch: 'full',
    component: SelfApplyingComponent,
  },
];

@NgModule({
  exports: [RouterModule],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class ApplicantsRoutingModule { }
