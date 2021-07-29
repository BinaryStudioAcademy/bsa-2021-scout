import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RoutingModule } from '../routing/routing.module';
import { AppComponent } from './components/app/app.component';
import { ToastrModule } from 'ngx-toastr';
import { LoginComponent} from '../shared/components/login/login.component';
import { LogoBlockComponent } from '../shared/components/logo-block/logo-block.component';

@NgModule({
  declarations: [AppComponent, LoginComponent, LogoBlockComponent],
  imports: [
    BrowserModule,
    RoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    ToastrModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
