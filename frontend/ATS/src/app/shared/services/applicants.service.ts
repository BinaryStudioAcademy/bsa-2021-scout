import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateApplicant } from '../models/applicants/create-applicant';
import { UpdateApplicant } from '../models/applicants/update-applicant';
import { Applicant } from '../models/applicants/applicant';
import { HttpClientService } from './http-client.service';
import { MarkedApplicant } from 'src/app/shared/models/applicants/marked-applicant';
import { GetShortApplicant } from '../models/applicants/get-short-applicant';
import { FileUrl } from '../models/file/file';
import { CsvApplicant } from 'src/app/applicants/models/CsvApplicant';

@Injectable({ providedIn: 'root' })
export class ApplicantsService {
  constructor(private readonly httpClient: HttpClientService) {}

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
    return this.httpClient.deleteRequest<Applicant>(
      '/applicants/' + applicantId,
    );
  }

  public getCv(applicantId: string): Observable<FileUrl> {
    return this.httpClient.getRequest<FileUrl>(`/applicants/${applicantId}/cv`);
  }

  public getApplicantsFromCSV(file: File) {
    const fd = new FormData();
    fd.append('file', file, file.name);
    return this.httpClient.postFullRequest<CsvApplicant[]>('/applicants/csv', fd);
  }

  public addRangeApplicants(applicants: CreateApplicant[]) {
    return this.httpClient.postFullRequest<Applicant[]>('/applicants/range', applicants);
  }

}
