import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateApplicant } from '../models/applicants/create-applicant';
import { UpdateApplicant } from '../models/applicants/update-applicant';
import { Applicant } from '../models/applicants/applicant';
import { HttpClientService } from './http-client.service';

@Injectable({ providedIn: 'root' })
export class ApplicantsService {
  constructor(private readonly httpClient: HttpClientService) {}

  public getApplicants(): Observable<Applicant[]> {
    return this.httpClient.getRequest<Applicant[]>('/applicants');
  }

  public addApplicant(createApplicant: CreateApplicant): Observable<Applicant> {
    return this.httpClient.postRequest<Applicant>(
      '/applicants',
      createApplicant,
    );
  }

  public updateApplicant(
    updateApplicant: UpdateApplicant,
  ): Observable<Applicant> {
    return this.httpClient.putRequest<Applicant>(
      '/applicants',
      updateApplicant,
    );
  }

  public deleteApplicant(applicantId: string): Observable<Applicant> {
    return this.httpClient.deleteRequest<Applicant>(
      '/applicants/' + applicantId,
    );
  }
}
