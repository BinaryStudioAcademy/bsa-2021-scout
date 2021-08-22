import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FullVacancyCandidate } from '../models/vacancy-candidates/full';
import { VacancyCandidate } from '../models/vacancy-candidates/vacancy-candidate';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class VacancyCandidateService {
  public constructor(private readonly http: HttpClientService) {}

  public getFull(id: string): Observable<FullVacancyCandidate> {
    return this.http.getRequest<FullVacancyCandidate>(
      `/vacancyCandidates/${id}/full`,
    );
  }

  public changeCandidateStage(
    id: string,
    stageId: string,
  ): Observable<VacancyCandidate> {
    return this.http.putClearRequest(
      `/vacancyCandidates/${id}/set-stage/${stageId}`,
    );
  }
}
