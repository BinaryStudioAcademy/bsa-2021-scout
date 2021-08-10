import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { ForgotPasswordDto } from '../models/forgot-password-dto';
import { ResetPasswordDto } from '../models/reset-password-dto';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {


  constructor(private httpService: HttpClientService) {}

  public isEmailExist(email: string): Observable<boolean> {
    return this.httpService.getRequest<boolean>
    (`/Users/Email/${email}`);
  }

  public sendPasswordResetRequest(forgotPasswordDto: ForgotPasswordDto): Observable<void> {
    return this.httpService.postRequest<void>('/Auth/forgot-password', forgotPasswordDto);  
  }

  public isResetTokenPasswordValid(email: string, token: string): Observable<boolean> {
    return this.httpService.getRequest<boolean>
    (`/Auth/reset-password?email=${email}&token=${token}`);
  }

  public resetPassword(resetPasswordDto: ResetPasswordDto): Observable<void> {
    return this.httpService.postRequest<void>('/Auth/reset-password', resetPasswordDto);
  }
}
