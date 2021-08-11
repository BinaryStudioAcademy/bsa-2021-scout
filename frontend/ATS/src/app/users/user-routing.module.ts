import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ResetPasswordGuard } from './guards/reset-password.guard';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { AppRoute } from '../routing/AppRoute';
import { SuccessfulRegistrationComponent } from
  './components/successful-registration/successful-registration.component';

const routes: Routes = [
  {
    path: AppRoute.Login,
    pathMatch: 'full',
    component: LoginComponent,
  },
  { path: 'reset-password', component: ResetPasswordComponent, canActivate: [ResetPasswordGuard] },
  {
    path: AppRoute.ConfirmEmail,
    pathMatch: 'full',
    component: ConfirmEmailComponent,
  },
  {
    path: AppRoute.SuccessfulRegistration,
    pathMatch: 'full',
    component: SuccessfulRegistrationComponent,
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
export class UserRoutingModule { }
