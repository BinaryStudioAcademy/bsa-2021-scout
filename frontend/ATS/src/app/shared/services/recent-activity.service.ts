import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RecentActivityInfo } from '../models/candidate-to-stages/recent-activity-info';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class RecentActivityService {
  public constructor(private readonly http: HttpClientService) {}

  public getRecentActivity(page: number = 1): Observable<RecentActivityInfo> {
    return this.http.getRequest(`/recentActivity/${page}`);
  }
}
