import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AppRoute } from '../routing/AppRoute';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
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
//  { path: 'home', component: HomeComponent },
//  { path: 'pools', component: ApplicationPoolComponent },
];

@NgModule({
  declarations: [],
  exports: [RouterModule],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class UserRoutingModule {}
