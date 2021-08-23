import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfirmEmailDto } from '../../models/confirm-email-dto';
import { AuthenticationService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: [
    './confirm-email.component.scss',
    '../login-regist-common/login-regist-common.component.scss',
  ],
})
export class ConfirmEmailComponent implements OnInit, OnDestroy {
  constructor(
    public authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private notificationService: NotificationService,
    private router: Router,
  ) {}

  IsEmailConfirmed: boolean = false;
  public loading: boolean = true;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  ngOnInit(): void {
    const dto: ConfirmEmailDto = {
      email: this.route.snapshot.queryParamMap.get('email') as string,
      token: this.route.snapshot.queryParamMap.get('token') as string,
    };
    this.authenticationService
      .confirmEmail(dto)
      .pipe()
      .subscribe(
        () => {
          this.loading = false;
          this.IsEmailConfirmed = true;
        },
        (error) => {
          this.loading = false;
          this.router.navigate(['/login']);
          this.notificationService.showErrorMessage(error.description);
        },
      );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public toHome() {
    this.router.navigate(['/home']);
  }
}
