import { Injectable } from '@angular/core';
import moment from 'moment';
import { Observable, of } from 'rxjs';
import { StageWithCandidates } from '../models/stages/with-candidates';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class StageService {
  public constructor(private readonly http: HttpClientService) {}

  public getByVacancyIdWithCandidates(
    vacancyId: string,
  ): Observable<StageWithCandidates[]> {
    return this.http.getRequest<StageWithCandidates[]>(
      `/stages/by-vacancy/${vacancyId}`,
    );
  }
}
