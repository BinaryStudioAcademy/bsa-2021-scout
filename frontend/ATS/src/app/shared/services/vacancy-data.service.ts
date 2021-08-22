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
  public routePrefix: string = '/vacancies';

  constructor(private readonly http: HttpClientService) { }

  public getList(): Observable<VacancyData[]> {
    return this.http.getRequest<VacancyData[]>(
      this.routePrefix,
    );
  }

  public getVacancies(): Observable<ShortVacancyWithDepartment[]> {
    return this.http.getRequest<ShortVacancyWithDepartment[]>(
      this.routePrefix,
    );
  }

  public getNotAppliedVacanciesByApplicant(applicantId: string):
  Observable<ShortVacancyWithDepartment[]> {
    return this.http.getRequest<ShortVacancyWithDepartment[]>(
      this.routePrefix + `/applicant/${applicantId}`,
    );
  }

  public getVacancy(vacancyId: string): Observable<ShortVacancyWithDepartment> {
    return this.http.getRequest<ShortVacancyWithDepartment>(
      this.routePrefix + `${vacancyId}`,
    );
  }

  public deleteVacancy(vacancyId: string) {
    return this.http.deleteFullRequest<void>(`${this.routePrefix}/${vacancyId}`);
  }
}
