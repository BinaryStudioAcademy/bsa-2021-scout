import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';

import { AppRoute } from '../routing/AppRoute';

import { HomeComponent } from './components/home/home.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ResetPasswordGuard } from './guards/reset-password.guard';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';


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

  { path: AppRoute.ResetPassword, component: ResetPasswordComponent, 
    canActivate: [ResetPasswordGuard] },
];

@NgModule({
  declarations: [],
  exports: [RouterModule],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class UserRoutingModule {}