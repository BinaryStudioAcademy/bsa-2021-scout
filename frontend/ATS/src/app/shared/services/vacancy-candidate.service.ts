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

  public getFull(id: string, vacancyId: string): Observable<FullVacancyCandidate> {
    return this.http.getRequest<FullVacancyCandidate>(
      `/vacancyCandidates/${vacancyId}/${id}/full`,
    );
  }

  public changeCandidateStage(
    id: string,
    vacancyId: string,
    stageId: string,
  ): Observable<VacancyCandidate> {
    return this.http.putClearRequest(
      `/vacancyCandidates/${id}/set-stage/${vacancyId}/${stageId}`,
    );
  }

  public postRangeOfCandidates(vacancyId:string, applicantsIds: string[]){
    return this.http.postRequest<void>(
      `/VacancyCandidates/CandidatesRange/${vacancyId}`, applicantsIds,
    );
  }

  public PostVacancyCandidateNoAuth(vacancyId:string, applicantId: string){
    return this.http.postFullRequest<void>(
      `/VacancyCandidates/${vacancyId}/${applicantId}`,new Object());
  }

  public MarkAsViewed(candidateId: string){
    return this.http.postFullRequest<void>(`/VacancyCandidates/viewed/${candidateId}`,new Object());
  }
}
