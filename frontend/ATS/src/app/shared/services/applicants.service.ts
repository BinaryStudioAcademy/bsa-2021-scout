import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateApplicant } from '../models/applicant/create-applicant';
import { UpdateApplicant } from '../models/applicant/update-applicant';
import { Applicant } from '../models/applicant/applicant';
import { HttpClientService } from './http-client.service';
import { MarkedApplicant } from 'src/app/shared/models/applicant/marked-applicant';
import { GetShortApplicant } from '../models/applicant/get-short-applicant';

@Injectable({ providedIn: 'root' })
export class ApplicantsService {
  constructor(
    private readonly httpClient: HttpClientService)
  { }

  public getApplicants(): Observable<Applicant[]> {
    return this.httpClient.getRequest<Applicant[]>('/applicants');
  }

  public getApplicantByCompany(id: string): Observable<GetShortApplicant> {
    return this.httpClient.getRequest<GetShortApplicant>(`/applicants/company/${id}`);
  }

  public getMarkedApplicants(vacancyId: string): Observable<MarkedApplicant[]> {
    return this.httpClient.getRequest<MarkedApplicant[]>(`/applicants/marked/${vacancyId}`);
  }

  public addApplicant(createApplicant: CreateApplicant): Observable<Applicant> {
    return this.httpClient.postRequest<Applicant>('/applicants', createApplicant);
  }

  public updateApplicant(updateApplicant: UpdateApplicant): Observable<Applicant> {
    return this.httpClient.putRequest<Applicant>('/applicants', updateApplicant);
  }

  public deleteApplicant(applicantId: string): Observable<Applicant> {
    return this.httpClient.deleteRequest<Applicant>('/applicants/' + applicantId);
  }
}