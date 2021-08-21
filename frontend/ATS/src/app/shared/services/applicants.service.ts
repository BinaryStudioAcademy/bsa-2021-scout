import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateApplicant } from '../models/applicant/create-applicant';
import { UpdateApplicant } from '../models/applicant/update-applicant';
import { Applicant } from '../models/applicant/applicant';
import { HttpClientService } from './http-client.service';
import { MarkedApplicant } from 'src/app/shared/models/applicant/marked-applicant';
import { GetShortApplicant } from '../models/applicant/get-short-applicant';
import { File } from '../models/file/file';

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
    const formData = new FormData();
    formData.append('body', JSON.stringify(createApplicant));
    if (createApplicant.cv) {
      formData.append('cvFile', createApplicant.cv, createApplicant.cv.name);
    }
    return this.httpClient.postRequest<Applicant>('/applicants', formData);
  }

  public updateApplicant(updateApplicant: UpdateApplicant): Observable<Applicant> {
    const formData = new FormData();
    formData.append('body', JSON.stringify(updateApplicant));
    if (updateApplicant.cv) {
      formData.append('cvFile', updateApplicant.cv, updateApplicant.cv.name);
    }
    return this.httpClient.putRequest<Applicant>('/applicants', formData);
  }

  public deleteApplicant(applicantId: string): Observable<Applicant> {
    return this.httpClient.deleteRequest<Applicant>('/applicants/' + applicantId);
  }

  public getCv(applicantId: string): Observable<File> {
    return this.httpClient.getRequest<File>(`/applicants/${applicantId}/cv`);
  }
}