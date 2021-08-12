import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { HomeComponent } from './components/home/home.component';
import { ApplicationPoolComponent } from './components/application-pool/application-pool.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'applicants', component: ApplicationPoolComponent },
  {
    path: 'login',
    pathMatch: 'full',
    component: LoginComponent,
  },
  { path: 'registration', component: RegistrationComponent },
  { path: 'pools', component: ApplicationPoolComponent },
];

@NgModule({
  declarations: [],
  exports: [RouterModule],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class UserRoutingModule {}
