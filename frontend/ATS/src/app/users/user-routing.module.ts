import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';

import { AppRoute } from '../routing/AppRoute';

import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ResetPasswordGuard } from './guards/reset-password.guard';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';

import { SuccessfulRegistrationComponent } from
  './components/successful-registration/successful-registration.component';
import { LoggedInUserGuard } from './guards/logged-in-user.guard';
import { ResendEmailAfterLoginComponent } from
  './components/resend-email-after-login/resend-email-after-login.component';


const routes: Routes = [
  {
    path: AppRoute.Login,
    pathMatch: 'full',
    component: LoginComponent,
    canActivate: [LoggedInUserGuard],
  },
  {
    path: AppRoute.ConfirmEmail,
    pathMatch: 'full',
    component: ConfirmEmailComponent,
    canActivate: [LoggedInUserGuard],
  },
  {
    path: AppRoute.SuccessfulRegistration,
    pathMatch: 'full',
    component: SuccessfulRegistrationComponent,
    canActivate: [LoggedInUserGuard],
  },
  {
    path: AppRoute.Registration,
    pathMatch: 'full',
    component: RegistrationComponent,
    canActivate: [LoggedInUserGuard],
  },

  {
    path: AppRoute.ResetPassword, component: ResetPasswordComponent,
    canActivate: [ResetPasswordGuard],
  },
  {
    path: AppRoute.ResendEmail,
    pathMatch: 'full',
    component: ResendEmailAfterLoginComponent,
    canActivate: [LoggedInUserGuard],
  },
];

@NgModule({
  declarations: [],
  exports: [RouterModule],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class UserRoutingModule { }