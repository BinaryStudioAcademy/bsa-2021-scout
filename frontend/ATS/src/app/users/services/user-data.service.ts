import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { User } from '../models/user';

@Injectable()
export class UserDataService {

  constructor(private httpClientService: HttpClientService) { }
  
  public getUsersForHrLead(): Observable<User[]> {
    return this.httpClientService.getRequest<User[]>(
      '/Users/for-hr-lead',
    );
  }
}
