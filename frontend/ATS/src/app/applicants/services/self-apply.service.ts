import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getModifyApplicantFormData } from 'src/app/shared/helpers/modifyApplicant';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { CreateApplicant } from 'src/app/shared/models/applicants/create-applicant';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { ApplyTokenInfo } from '../models/ApplyTokenInfo';

@Injectable({
  providedIn: 'root',
})
export class SelfApplyService {

  constructor(private httpService: HttpClientService) { }

  public sendApplyConfirmEmail(applyInfo: ApplyTokenInfo): Observable<void> {
    return this.httpService.postRequest<void>('/SelfApply/email-confirm-apply', applyInfo);
  }

  public MarkTokenUsed(token: string): Observable<void> {
    return this.httpService.postRequest<void>(`/SelfApply/mark-token-used/${token}`, new Object());
  }

  public getApplyConfirmEmailToken(email: string, vacancyId: string): Observable<string[]> {
    return this.httpService
      .getRequest<string[]>(`/SelfApply/email-confirm-apply/${vacancyId}/${email}`);
  }

  public addSelfAppliedApplicant(
    createApplicant: CreateApplicant,
    vacancyId: string,
  ): Observable<Applicant> {
    return this.httpService.postRequest<Applicant>(
      `/SelfApply/${vacancyId}`,
      getModifyApplicantFormData(createApplicant),
    );
  }
}
