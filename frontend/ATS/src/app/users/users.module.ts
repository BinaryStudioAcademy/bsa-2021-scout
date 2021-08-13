import { NgModule, LOCALE_ID } from '@angular/core';
import { UserRoutingModule } from './user-routing.module';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './components/login/login.component';
import { LogoBlockComponent } from './components/logo-block/logo-block.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginBoxComponent } from './components/login-box/login-box.component';
import { RegistrationBoxComponent } from './components/registration-box/registration-box.component';
import { CreateTalentpoolModalComponent } 
  from './components/create-talentpool-modal/create-talentpool-modal.component';
import { EditAppPoolModalComponent } 
  from './components/edit-app-pool-modal/edit-app-pool-modal.component';
import { LoginRegistCommonComponent } 
  from './components/login-regist-common/login-regist-common.component';
import { HomeComponent } from './components/home/home.component';
import { ApplicationPoolComponent } from './components/application-pool/application-pool.component';
import { StylePaginatorDirective } from '../shared/directives/style-paginator.directive';

@NgModule({
  declarations: [
    LoginBoxComponent,
    LoginComponent,
    LoginRegistCommonComponent,
    LogoBlockComponent,
    RegistrationComponent,
    RegistrationBoxComponent,
    StylePaginatorDirective,
    CreateTalentpoolModalComponent,
    EditAppPoolModalComponent,
    ApplicationPoolComponent,
    
  ],
  imports: [UserRoutingModule, SharedModule],
  providers: [
    LoginRegistCommonComponent,
    { provide: LOCALE_ID, useValue: 'en-GB' },
  ],
})
export class UsersModule {}
