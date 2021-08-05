import { NgModule, LOCALE_ID } from '@angular/core';
import { UserRoutingModule } from './user-routing.module';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './components/login/login.component';
import { LogoBlockComponent } from './components/logo-block/logo-block.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginBoxComponent } from './components/login-box/login-box.component';
import { RegistrationBoxComponent } from './components/registration-box/registration-box.component';

// This line can't be shorter
// eslint-disable-next-line
import { LoginRegistCommonComponent } from './components/login-regist-common/login-regist-common.component';

@NgModule({
  declarations: [
    LoginBoxComponent,
    LoginComponent,
    LoginRegistCommonComponent,
    LogoBlockComponent,
    RegistrationComponent,
    RegistrationBoxComponent,
  ],
  imports: [UserRoutingModule, SharedModule],
  providers: [
    LoginRegistCommonComponent,
    { provide: LOCALE_ID, useValue: 'en-GB' },
  ],
})
export class UsersModule {}
