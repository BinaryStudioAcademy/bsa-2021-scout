import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { RegistrationPermissionShort } from '../models/registration-permission-short';

@Injectable()
export class RegistrationPermissionsService {
  constructor(private readonly httpClientService: HttpClientService)
  { }

  public getPendingRegistrations(): Observable<RegistrationPermissionShort[]> {
    return this.httpClientService
      .getRequest<RegistrationPermissionShort[]>('/Users/pending-registrations');
  }

  public resendRegistrationLink(registrationPermission: RegistrationPermissionShort)
    : Observable<RegistrationPermissionShort> {
    return this.httpClientService
      .putRequest('/Register/resend-registration-link', registrationPermission);
  }

  public revokeRegistrationLink(permissionId: string): Observable<void> {
    return this.httpClientService
      .deleteRequest<void>('/Register/revoke-registration-link/' + permissionId);
  }
}