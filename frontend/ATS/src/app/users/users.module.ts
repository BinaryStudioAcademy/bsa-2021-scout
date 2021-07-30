import { NgModule } from '@angular/core';
import { UserRoutingModule } from './user-routing.module';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './components/login/login.component';
import { LogoBlockComponent } from './components/logo-block/logo-block.component';


@NgModule({
  declarations: [LoginComponent, LogoBlockComponent],
  imports: [
    UserRoutingModule,
    SharedModule,
  ],
})
export class UsersModule { }
