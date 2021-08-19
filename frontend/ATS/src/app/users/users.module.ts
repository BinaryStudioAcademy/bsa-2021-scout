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
import { SuccessfulRegistrationComponent }
  from './components/successful-registration/successful-registration.component';
import { UserDataService } from './services/user-data.service';
import { UsersTableComponent } from './components/hr-lead/users-table/users-table.component';
import { LoggedInUserGuard } from './guards/logged-in-user.guard';
import { ResendEmailAfterLoginComponent } from
  './components/resend-email-after-login/resend-email-after-login.component';
import { ApplicationPoolComponent } from './components/application-pool/application-pool.component';
import { CreateTalentpoolModalComponent } 
  from './components/create-talentpool-modal/create-talentpool-modal.component';
import { EditAppPoolModalComponent } 
  from './components/edit-app-pool-modal/edit-app-pool-modal.component';


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
    SuccessfulRegistrationComponent,
    UsersTableComponent,
    ResendEmailAfterLoginComponent,
    SuccessfulRegistrationComponent,    
    ApplicationPoolComponent,
    CreateTalentpoolModalComponent,
    EditAppPoolModalComponent,    
  ],
  imports: [UserRoutingModule, SharedModule],
  providers: [
    LoginRegistCommonComponent,
    ResetPasswordGuard,
    UserDataService,
    LoggedInUserGuard,
  ],
})
export class UsersModule { }
