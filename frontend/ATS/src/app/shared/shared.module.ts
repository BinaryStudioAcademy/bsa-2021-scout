import { NgModule } from '@angular/core';
import { HttpClientService } from './services/http-client.service';

@NgModule({
  providers:[HttpClientService],
})
export class SharedModule {}
