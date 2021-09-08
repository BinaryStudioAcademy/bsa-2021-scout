import { Injectable } from '@angular/core';
import { ProjectInfo } from '../models/project-info';
import { HttpClientService } from '../../shared/services/http-client.service';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  public routePrefix = '/projects';

  constructor(private httpService: HttpClientService) { }

  public getProjects() {
    return this.httpService.getFullRequest<ProjectInfo[]>(`${this.routePrefix}`);
  }

  public getProject(id: number) {
    return this.httpService.getFullRequest<ProjectInfo>(`${this.routePrefix}/${id}`);
  }

  public createProject(post: ProjectInfo) {
    return this.httpService.postFullRequest<ProjectInfo>(`${this.routePrefix}`, post);
  }

  public updateProject(project: ProjectInfo) {
    return this.httpService.putFullRequest<ProjectInfo>(`${this.routePrefix}`, project);
  }
}