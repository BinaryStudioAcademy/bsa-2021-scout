import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { User } from '../models/user';
import { UserCreate } from '../models/user-create';
import { UserTableData } from '../models/user-table-data';

@Injectable()
export class UserDataService {

  constructor(private httpClientService: HttpClientService) { }
  
  public getUsersForHrLead(): Observable<UserTableData[]> {
    return this.httpClientService.getRequest<UserTableData[]>(
      '/Users/for-hr-lead',
    );
  }

  public getById(id:string):Observable<User>{
    return this.httpClientService.getRequest<User>('/users/'+id);
  }

  public postUser(user:UserCreate): Observable<UserCreate> {
    return this.httpClientService.postRequest<UserCreate>(
      '/users',
      user,
    );
  }

  public putUser(user:UserCreate, id:string): Observable<UserCreate> {
    return this.httpClientService.putRequest<UserCreate>(
      '/users/'+id,
      user,
    );
  }
}
