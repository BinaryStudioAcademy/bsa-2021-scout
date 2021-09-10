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

import {
  ForgotPasswordDialogComponent,
} from './components/forgot-password-dialog/forgot-password-dialog.component';

import {
  ResetPasswordBoxComponent,
} from './components/reset-password-box/reset-password-box.component';
// eslint-disable-next-line
import { LoginRegistCommonComponent } from './components/login-regist-common/login-regist-common.component';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';

import {
  SuccessfulRegistrationComponent,
} from './components/successful-registration/successful-registration.component';

import { UserDataService } from './services/user-data.service';
import { UsersTableComponent } from './components/hr-lead/users-table/users-table.component';
import { LoggedInUserGuard } from './guards/logged-in-user.guard';

import { HomeWidgetComponent } from './components/home-page/home-widget/home-widget.component';
import { HomeComponent } from './components/home-page/home/home.component';
import { HomeDataService } from './services/home-data.service';
import { VacancyCardComponent } from './components/home-page/vacancy-card/vacancy-card.component';

import {
  ResendEmailAfterLoginComponent, 
} from './components/resend-email-after-login/resend-email-after-login.component';

import {
  SendingRegisterLinkDialogComponent,
} from './components/hr-lead/send-registration-link-dialog/sending-register-link-dialog.component';

import { RecentActivityComponent } from './components/recent-activity/recent-activity.component';
import { EditHrFormComponent } from './components/edit-hr-form/edit-hr-form.component';
import { PendingRegistrationsComponent }
  from './components/hr-lead/pending-registrations/pending-registrations.component';
import { RegistrationPermissionsService } from './services/registration-permissions.service';
import { MyTasksComponent } from '../task-management/components/my-tasks/my-tasks.component';

@NgModule({
  declarations: [
    LoginBoxComponent,
    LoginComponent,
    LoginRegistCommonComponent,
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
    SendingRegisterLinkDialogComponent,
    HomeWidgetComponent,
    HomeComponent,
    VacancyCardComponent,
    RecentActivityComponent,
    EditHrFormComponent,
    PendingRegistrationsComponent,
    MyTasksComponent,
  ],
  imports: [UserRoutingModule, SharedModule],
  providers: [
    LoginRegistCommonComponent,
    ResetPasswordGuard,
    UserDataService,
    LoggedInUserGuard,
    HomeDataService,
    RegistrationPermissionsService,
  ],
})
export class UsersModule {}
