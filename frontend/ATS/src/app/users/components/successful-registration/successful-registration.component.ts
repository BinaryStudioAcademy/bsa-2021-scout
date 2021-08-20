import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { environment } from 'src/environments/environment';
import { ResendConfirmEmailDto } from '../../models/resend-confirm-email-dto';
import { AuthenticationService } from '../../services/auth.service';

@Component({
  selector: 'app-successful-registration',
  templateUrl: './successful-registration.component.html',
  styleUrls: ['./successful-registration.component.scss',
    '../login-regist-common/login-regist-common.component.scss'],
})
export class SuccessfulRegistrationComponent {

  constructor(public authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private notificationService: NotificationService) { }

  public resendEmail(): void {
    const dto: ResendConfirmEmailDto = {
      email: this.route.snapshot.queryParamMap.get('email') as string,
      clientUrl: `${environment.clientUrl}/confirm-email`,
    };
    this.authenticationService.resendConfirmationEmail(dto).pipe()
      .subscribe(() => {
        this.notificationService.showSuccessMessage(
          'Please check your email to confirm your email.');
      },
      (error) => {
        if (error.description != null) {
          this.notificationService.showErrorMessage(error.description, 'Something went wrong');
        }
        else {
          this.notificationService.showErrorMessage(error, 'Something went wrong');
        }
      });
  }

}
