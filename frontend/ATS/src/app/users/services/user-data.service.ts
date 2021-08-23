import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { UserTableData } from '../models/user-table-data';

@Injectable()
export class UserDataService {

  constructor(private httpClientService: HttpClientService) { }
  
  public getUsersForHrLead(): Observable<UserTableData[]> {
    return this.httpClientService.getRequest<UserTableData[]>(
      '/Users/for-hr-lead',
    );
  }
}
