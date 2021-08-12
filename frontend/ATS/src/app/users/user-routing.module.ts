import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { AppRoute } from '../routing/AppRoute';
import { HomeComponent } from './components/home/home.component';
import { ApplicationPoolComponent } from './components/application-pool/application-pool.component';

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
  { 
    path: AppRoute.Home,
    pathMatch: 'full', 
    component: HomeComponent, 
  },
  { 
    path: AppRoute.Pools,
    pathMatch: 'full', 
    component: ApplicationPoolComponent, 
  },
];

@NgModule({
  declarations: [],
  exports: [RouterModule],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class UserRoutingModule {}
