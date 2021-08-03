import { NgModule } from '@angular/core';
import { HttpClientService } from './services/http-client.service';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import {MatDividerModule} from '@angular/material/divider';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';



@NgModule({
  exports: [ 
    MatButtonModule, 
    MatFormFieldModule, 
    MatInputModule, 
    MatIconModule,
    MatDividerModule,
    ReactiveFormsModule, 
    CommonModule,
  ],
  providers:[HttpClientService],
  declarations: [
  ],  
})
export class SharedModule {}
