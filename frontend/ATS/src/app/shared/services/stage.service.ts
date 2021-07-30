import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ShortVacancyWithStages } from '../models/vacancy/short-with-stages';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class StageService {
  public constructor(private readonly http: HttpClientService) {}

  public getByVacancyIdWithCandidates(
    vacancyId: string,
  ): Observable<ShortVacancyWithStages> {
    return this.http.getRequest<ShortVacancyWithStages>(
      `/stages/by-vacancy/${vacancyId}`,
    );
  }
}
