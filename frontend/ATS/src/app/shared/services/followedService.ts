import { EntityType } from './../enums/entity-type.enum';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApplicantsPool } from '../models/applicants-pool/applicants-pool';
import { CreatePool } from '../models/applicants-pool/create-pool';
import { UpdatePool } from '../models/applicants-pool/update-pool';
import { Followed } from '../models/followed/create-followed';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class FollowedService {
  public constructor(private readonly http: HttpClientService) { }
  public routePrefix = '/UserFollowed';

  public getFollowed(type: EntityType) {
    return this.http.getRequest<Followed[]>(`${this.routePrefix}/${type}`);
  }
  public switchRefreshFollowedPageToken(page: string, token: string) {
    localStorage.setItem(token, page);
  }
  public createFollowed(follow: Followed): Observable<Followed> {
    return this.http.postRequest<Followed>(
      `${this.routePrefix}`, follow);
  }
  public deleteFollowed(type: EntityType, id: string) {
    return this.http.deleteFullRequest<Followed>(`${this.routePrefix}/${id}/${type}`);
  }
}
