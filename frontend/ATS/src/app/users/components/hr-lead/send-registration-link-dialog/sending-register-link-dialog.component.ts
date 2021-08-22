import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { finalize } from 'rxjs/operators';
import { AppRoute } from 'src/app/routing/AppRoute';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { RegistrationLinkDto } from 'src/app/users/models/registration-link-dto';
import { AuthenticationService } from 'src/app/users/services/auth.service';
import { environment } from 'src/environments/environment';
import { LoginRegistCommonComponent } 
  from '../../login-regist-common/login-regist-common.component';


@Component({
  selector: 'app-sending-register-link-dialog',
  templateUrl: './sending-register-link-dialog.component.html',
  styleUrls: ['./sending-register-link-dialog.component.scss'],
})
export class SendingRegisterLinkDialogComponent {

  constructor(
    private dialogRef: MatDialogRef<SendingRegisterLinkDialogComponent>,
    public loginRegistCommonComponent: LoginRegistCommonComponent,
    private notificationService: NotificationService,
    private authService: AuthenticationService) { }

  public isRequestFinished = true;

  public emailForm: FormGroup = new FormGroup({
    'userEmail': new FormControl('', [
      Validators.required,
      Validators
        .pattern(
          '^([a-zA-Z0-9_-]+\.)*[a-zA-Z0-9_-]+@[a-zA-Z0-9_]+(\.[a-zA-Z0-9_-]+)*\.[a-zA-Z]{1,6}$',
        ),
      this.loginRegistCommonComponent.noUnAllowedCharactersValidation,
    ]),
  });

  public sendRegistLink(): void {
    if (this.emailForm.valid) {
      this.isRequestFinished = false;
      const registrationLinkDto: RegistrationLinkDto =
      {
        email: this.emailForm.get('userEmail')?.value,
        clientURI: `${environment.clientUrl}/${AppRoute.Registration}`,
      };
      this.authService.sendRegistrationLink(registrationLinkDto)
        .pipe(
          finalize(() => this.isRequestFinished = true),
        )
        .subscribe(() => {
          this.notificationService.showSuccessMessage(
            'Message sent successfully');
          this.dialogRef.close();
        },
        () => this.notificationService.showErrorMessage('Something went wrong'));
    }

  }
}