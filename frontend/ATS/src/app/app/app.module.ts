import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { RoutingModule } from '../routing/routing.module';
import { AppComponent } from './components/app/app.component';
import { ToastrModule } from 'ngx-toastr';
import { LoginComponent} from '../users/components/login/login.component';
import { LogoBlockComponent } from '../users/components/logo-block/logo-block.component';
import { ReactiveFormsModule } from '@angular/forms';

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
    MatIconModule,
    ToastrModule.forRoot(),
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
