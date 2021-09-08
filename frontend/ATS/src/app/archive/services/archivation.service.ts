import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { ArchivedProject } from '../models/archived-project';
import { ArchivedVacancy } from '../models/archived-vacancy';

@Injectable({ providedIn: 'root' })
export class ArchivationService {
  private controllerName = 'archive';

  constructor(private httpClientService: HttpClientService) { }
  
  public archiveVacancy(vacancyId: string, isClosed: boolean = false): Observable<void> {
    return this.httpClientService.putRequest(
      `/${this.controllerName}/vacancies/${vacancyId}${isClosed ? '/closed' : ''}`,
      new Object(),
    );
  }

  public archiveProject(projectId: string): Observable<void> {
    return this.httpClientService.putRequest(
      `/${this.controllerName}/projects/${projectId}`,
      new Object(),
    );
  }

  public unarchiveVacancy(vacancy: ArchivedVacancy): Observable<void> {
    return this.httpClientService.putRequest(
      `/${this.controllerName}/unarchive/vacancies`,
      vacancy,
    );
  }

  public unarchiveProject(project: ArchivedProject): Observable<void> {
    return this.httpClientService.putRequest(
      `/${this.controllerName}/unarchive/projects`,
      project,
    );
  }
}