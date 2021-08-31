import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class ApplicantCvService {
  public constructor(private readonly http: HttpClientService) {}

  public startCvParsing(file: File): Observable<void> {
    const formData: FormData = new FormData();
    formData.append('file', file);

    return this.http.postRequest('/applicantCv/file-to-applicant', formData);
  }
}
