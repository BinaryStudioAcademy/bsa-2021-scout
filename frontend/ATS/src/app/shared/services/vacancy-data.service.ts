import { VacancyData } from 'src/app/shared/models/vacancy/vacancy-data';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from './http-client.service';
import { map } from 'rxjs/operators';
import { VacancyStatus } from '../models/vacancy/vacancy-status';
import { ShortVacancyWithDepartment } 
  from 'src/app/shared/models/vacancy/short-vacancy-with-department';

@Injectable({
  providedIn: 'root',
})
export class VacancyDataService {

  constructor(private readonly http: HttpClientService) { }
  
  public getList(): Observable<VacancyData[]> {
    return this.http.getRequest<VacancyData[]>(
      '/vacancies',
    );
  }

  public getVacancies(): Observable<ShortVacancyWithDepartment[]> {
    return this.http.getRequest<ShortVacancyWithDepartment[]>(
      '/vacancies',
    );
  }

  public getNotAppliedVacanciesByApplicant(applicantId: string): 
  Observable<ShortVacancyWithDepartment[]> {
    return this.http.getRequest<ShortVacancyWithDepartment[]>(
      `/vacancies/applicant/${applicantId}`,
    );
  }

  public getVacancy(vacancyId: string): Observable<ShortVacancyWithDepartment> {
    return this.http.getRequest<ShortVacancyWithDepartment>(
      `/vacancies/${vacancyId}`,
    );
  }
}
