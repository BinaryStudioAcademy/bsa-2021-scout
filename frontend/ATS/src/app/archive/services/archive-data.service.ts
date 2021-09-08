import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { ArchivedProject } from '../models/archived-project';
import { ArchivedVacancy } from '../models/archived-vacancy';

@Injectable()
export class ArchiveDataService {
  private controllerName = 'archive';

  constructor(private httpClientService: HttpClientService) { }

  public getArchivedProjects(): Observable<ArchivedProject[]> {
    return this.httpClientService.getRequest<ArchivedProject[]>(
      `/${this.controllerName}/projects`,
    );
  }

  public getArchivedVacancies(): Observable<ArchivedVacancy[]> {
    return this.httpClientService.getRequest<ArchivedVacancy[]>(
      `/${this.controllerName}/vacancies`,
    );
  }

  public deleteProject(vacancy: ArchivedProject): Observable<void> {
    return this.httpClientService.deleteRequest(
      `/${this.controllerName}/projects/${vacancy.id}`,
    );
  }

  public deleteVacancy(vacancy: ArchivedVacancy): Observable<void> {
    return this.httpClientService.deleteRequest(
      `/${this.controllerName}/vacancies/${vacancy.id}`,
    );
  }
}