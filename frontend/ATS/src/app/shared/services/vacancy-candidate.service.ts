import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { VacancyCandidate } from '../models/vacancy-candidates/vacancy-candidate';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class VacancyCandidateService {
  public constructor(private readonly http: HttpClientService) {}

  public changeCandidateStage(
    id: string,
    stageId: string,
  ): Observable<VacancyCandidate> {
    return this.http.putClearRequest(
      `/vacancyCandidates/${id}/set-stage/${stageId}`,
    );
  }
}
