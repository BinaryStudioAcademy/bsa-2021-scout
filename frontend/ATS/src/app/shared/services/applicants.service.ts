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
import { VacancyWithRecentActivity }
  from '../models/candidate-to-stages/vacancy-with-recent-activity';
import { getModifyApplicantFormData } from '../helpers/modifyApplicant';

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
    return this.httpClient.postRequest<Applicant>(
      '/applicants',
      getModifyApplicantFormData(createApplicant),
    );
  }

  public updateApplicant(updateApplicant: UpdateApplicant): Observable<Applicant> {
    return this.httpClient.putRequest<Applicant>(
      '/applicants',
      getModifyApplicantFormData(updateApplicant),
    );
  }

  public deleteApplicant(applicantId: string): Observable<Applicant> {
    return this.httpClient.deleteRequest<Applicant>(
      '/applicants/' + applicantId,
    );
  }

  public getCv(applicantId: string): Observable<FileUrl> {
    return this.httpClient.getRequest<FileUrl>(`/applicants/${applicantId}/cv`);
  }

  public getApplicantByEmail(email: string): Observable<Applicant>{
    return this.httpClient.getRequest<Applicant>(`/applicants/property/email/${email}`);
  }

  public getRecentActivity(id: string): Observable<VacancyWithRecentActivity[]> {
    return this.httpClient.getRequest<VacancyWithRecentActivity[]>(
      `/recentActivity/for-applicant/${id}`,
    );
  }
}
