import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { VacancyCreate } from '../models/vacancy/vacancy-create';
import { VacancyFull } from '../models/vacancy/vacancy-full';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class VacancyService {

  public constructor(private readonly http: HttpClientService) {}

  public getById(id:string):Observable<VacancyFull>{
    return this.http.getRequest<VacancyFull>('/vacancies/'+id);
  }

  public postVacancy(vacancy:VacancyCreate): Observable<VacancyFull> {
    return this.http.postRequest<VacancyFull>(
      '/vacancies',
      vacancy,
    );
  }

  public putVacancy(vacancy:VacancyCreate, id:string): Observable<VacancyFull> {
    return this.http.putRequest<VacancyFull>(
      '/vacancies/'+id,
      vacancy,
    );
  }
}