import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Project } from '../models/projects/project';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {

  public constructor(private readonly http: HttpClientService) {}

  public getByCompany(): Observable<Project[]> {
    return this.http.getRequest<Project[]>(
      '/users/current/company/projects',
    );
  }
}