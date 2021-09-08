import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { HotVacancySummary } from '../models/home/hot-vacancy-summary';
import { WidgetsData } from '../models/home/widgets-data';

@Injectable()
export class HomeDataService {
  private routePrefix = '/home';
  constructor(private httpClientService: HttpClientService) { }
  
  public GetWidgetsData(): Observable<WidgetsData> {
    return this.httpClientService.getRequest<WidgetsData>(
      `${this.routePrefix}/widgets-data`,
    );
  }

  public GetHotVacanciesSummary(): Observable<HotVacancySummary[]> {
    return this.httpClientService.getRequest<HotVacancySummary[]>(
      `${this.routePrefix}/vacancies`,
    );
  }
}
