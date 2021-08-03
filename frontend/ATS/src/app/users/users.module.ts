import { NgModule } from '@angular/core';
import { UserRoutingModule } from './user-routing.module';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './components/login/login.component';
import { LogoBlockComponent } from './components/logo-block/logo-block.component';
import { LoginBoxComponent } from './components/login-box/login-box.component';
import { LoginRegistCommonComponent } 
  from './components/login-regist-common/login-regist-common.component';



@NgModule({
  declarations: [LoginBoxComponent, LoginComponent, LoginRegistCommonComponent, LogoBlockComponent],
  imports: [
    UserRoutingModule,
    SharedModule,
  ],
  providers: [LoginRegistCommonComponent],
})
export class UsersModule { }
