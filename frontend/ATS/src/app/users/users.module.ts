import { NgModule, LOCALE_ID } from '@angular/core';
import { UserRoutingModule } from './user-routing.module';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './components/login/login.component';
import { LogoBlockComponent } from './components/logo-block/logo-block.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginBoxComponent } from './components/login-box/login-box.component';
import { RegistrationBoxComponent } from './components/registration-box/registration-box.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ResetPasswordGuard } from './guards/reset-password.guard';
import { ForgotPasswordDialogComponent } 
  from './components/forgot-password-dialog/forgot-password-dialog.component';
import { ResetPasswordBoxComponent } 
  from './components/reset-password-box/reset-password-box.component';

// This line can't be shorter
// eslint-disable-next-line
import { LoginRegistCommonComponent } from './components/login-regist-common/login-regist-common.component';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';

@NgModule({
  declarations: [
    LoginBoxComponent,
    LoginComponent,
    LoginRegistCommonComponent,
    LogoBlockComponent,
    RegistrationComponent,
    RegistrationBoxComponent,
    ForgotPasswordDialogComponent,
    ResetPasswordComponent,
    ResetPasswordBoxComponent,
    ConfirmEmailComponent,
  ],
  imports: [UserRoutingModule, SharedModule],
  providers: [
    LoginRegistCommonComponent,
    { provide: LOCALE_ID, useValue: 'en-GB' },
    ResetPasswordGuard,
  ],
})
export class UsersModule {}
