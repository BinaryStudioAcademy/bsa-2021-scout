import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getModifyApplicantFormData } from 'src/app/shared/helpers/modifyApplicant';
import { CreateApplicant } from 'src/app/shared/models/applicants/create-applicant';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { CsvApplicant } from '../models/CsvApplicant';
import { CsvFile } from '../models/CsvFile';

@Injectable({
  providedIn: 'root',
})
export class ApplicantCsvService {

  constructor(private readonly httpClient: HttpClientService) { }

  public getApplicantsFromCSV(file: File) {
    const fd = new FormData();
    fd.append('file', file, file.name);
    return this.httpClient.postFullRequest<CsvApplicant[]>('/applicantCsv/csv', fd);
  }

  public addRangeCsvApplicants(applicants: CreateApplicant[]) {
    return this.httpClient.postFullRequest<CsvApplicant[]>('/applicantCsv/range', applicants);
  }

  public postCsvFile(file: CsvFile){
    const formData = new FormData();
    formData.append('body',JSON.stringify(file));
    return this.httpClient.postFullRequest<CsvFile>('/applicantCsv/file', formData);
  }

  public putCsvFile(file: CsvFile){
    const formData = new FormData();
    formData.append('body',JSON.stringify(file));
    return this.httpClient.putFullRequest<CsvFile>('/applicantCsv/file', formData);
  }

  public getCsvFiles(){
    return this.httpClient.getFullRequest<CsvFile[]>('/applicantCsv');
  }

  public addCsvApplicant(createApplicant: CreateApplicant): Observable<CsvApplicant> {
    return this.httpClient.postRequest<CsvApplicant>(
      '/applicantCsv/csvApplicant',
      getModifyApplicantFormData(createApplicant),
    );
  }
}
