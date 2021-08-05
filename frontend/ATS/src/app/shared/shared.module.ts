import { NgModule } from '@angular/core';
import { HttpClientService } from './services/http-client.service';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatToolbarModule} from '@angular/material/toolbar';
import { MatBadgeModule} from '@angular/material/badge';
import { MatSidenavModule } from '@angular/material/sidenav';
import { FormsModule } from '@angular/forms';
import { ButtonComponent } from './components/button/button.component';
import { HeaderComponent } from './components/header/header.component';
import { SearchFormComponent } from './components/search-form/search-form.component';
import { MatIconModule } from '@angular/material/icon';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatListModule} from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { MultiselectComponent } from './components/multiselect/multiselect.component';



@NgModule({
  exports: [ 
    MatButtonModule, 
    MatFormFieldModule,
    MatInputModule,
    MatToolbarModule,
    MatIconModule,
    MatBadgeModule,
    MatSidenavModule,
    FormsModule,
    ButtonComponent,
    SearchFormComponent,
    ReactiveFormsModule,
    CommonModule,
    BrowserModule, 
    FormsModule, 
    BrowserAnimationsModule,
    HeaderComponent,
    MatListModule,
    MatSelectModule,
    MultiselectComponent,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  imports:[
    MatButtonModule, 
    MatInputModule,
    MatToolbarModule,
    MatIconModule,
    MatBadgeModule,
    MatSidenavModule,
    FormsModule,
    CommonModule,
    MatFormFieldModule, 
    MatInputModule, 
    MatIconModule, 
    MatDatepickerModule,
    MatNativeDateModule,
    MatListModule,
    BrowserAnimationsModule,
    BrowserModule, 
    MatSelectModule,
    ReactiveFormsModule,
  ],
  providers:[HttpClientService],
  declarations: 
  [
    MultiselectComponent,
    ButtonComponent,
    SearchFormComponent,
    HeaderComponent,
  ],
})
export class SharedModule {}
