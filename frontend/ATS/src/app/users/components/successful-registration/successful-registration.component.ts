import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { environment } from 'src/environments/environment';
import { ResendConfirmEmailDto } from '../../models/resend-confirm-email-dto';
import { AuthenticationService } from '../../services/auth.service';

@Component({
  selector: 'app-successful-registration',
  templateUrl: './successful-registration.component.html',
  styleUrls: [
    './successful-registration.component.scss',
    '../login-regist-common/login-regist-common.component.scss',
  ],
})
export class SuccessfulRegistrationComponent implements OnDestroy {
  public loading: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private notificationService: NotificationService,
  ) {}

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public resendEmail(): void {
    const dto: ResendConfirmEmailDto = {
      email: this.route.snapshot.queryParamMap.get('email') as string,
      clientUrl: `${environment.clientUrl}/confirm-email`,
    };
    this.loading = true;
    this.authenticationService
      .resendConfirmationEmail(dto)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        () => {
          this.loading = false;

          this.notificationService.showSuccessMessage(
            'Please check your email to confirm your email.',
          );
        },
        (error) => {
          this.loading = false;

          if (error.description != null) {
            this.notificationService.showErrorMessage(
              error.description,
              'Something went wrong',
            );
          } else {
            this.notificationService.showErrorMessage(
              error,
              'Something went wrong',
            );
          }
        },
      );
  }
}
