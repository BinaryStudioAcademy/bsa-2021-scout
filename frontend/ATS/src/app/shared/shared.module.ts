import { NgModule } from '@angular/core';
import { HttpClientService } from './services/http-client.service';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatIconModule} from '@angular/material/icon';
import {MatBadgeModule} from '@angular/material/badge';
import { FormsModule } from '@angular/forms';
import { ButtonComponent } from './components/button/button.component';
import { CommonModule } from '@angular/common';
import { SearchFormComponent } from './components/search-form/search-form.component';

@NgModule({
  exports: [ 
    MatButtonModule, 
    MatFormFieldModule,
    MatInputModule,
    MatToolbarModule,
    MatIconModule,
    MatBadgeModule,
    FormsModule,
    ButtonComponent,
    SearchFormComponent
  ],
  imports:[MatButtonModule, 
    MatFormFieldModule,
    MatInputModule,
    MatToolbarModule,
    MatIconModule,
    MatBadgeModule,
    FormsModule,
    CommonModule],
  providers:[HttpClientService],
  declarations: [
    ButtonComponent,
    SearchFormComponent
  ],
})
export class SharedModule {}
