import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApplicantsPool } from '../models/applicants-pool/applicants-pool';
import { CreatePool } from '../models/applicants-pool/create-pool';
import { UpdatePool } from '../models/applicants-pool/update-pool';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class PoolService {
  public constructor(private readonly http: HttpClientService) {}
  public routePrefix = '/pools';

  public getPools() {
    return this.http.getRequest<ApplicantsPool[]>(
      `${this.routePrefix}`);
  }

  public createPool(pool : CreatePool) {
    return this.http.postRequest<ApplicantsPool>(
      `${this.routePrefix}`, pool);
  }

  public updatePool(pool : UpdatePool) {
    return this.http.putFullRequest<ApplicantsPool>(
      `${this.routePrefix}`, pool);
  }

}
