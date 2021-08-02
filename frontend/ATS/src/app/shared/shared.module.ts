import { NgModule } from '@angular/core';
import { HttpClientService } from './services/http-client.service';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MultiselectComponent } from './components/multiselect/multiselect.component';

@NgModule({
  imports: [MatSelectModule, ReactiveFormsModule, CommonModule],
  declarations: [MultiselectComponent],
  exports: [
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatSelectModule,
    ReactiveFormsModule,
    CommonModule,
    MultiselectComponent,
  ],
  providers: [HttpClientService],
})
export class SharedModule {}
