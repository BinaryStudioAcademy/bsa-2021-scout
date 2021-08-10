import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { AppRoute } from '../routing/AppRoute';

const routes: Routes = [
  {
    path: AppRoute.Login,
    pathMatch: 'full',
    component: LoginComponent,
  },
  { 
    path: AppRoute.Registration,
    pathMatch: 'full', 
    component: RegistrationComponent, 
  },
];

@NgModule({
  declarations: [],
  exports: [RouterModule],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class UserRoutingModule {}
