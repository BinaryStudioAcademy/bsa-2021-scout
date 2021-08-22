import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LoginRegistCommonComponent } from '../login-regist-common/login-regist-common.component';
import { RegisterDto } from '../../models/register-dto';
import { AuthenticationService } from '../../services/auth.service';
import { UserRegisterDto } from '../../models/auth/user-register-dto';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, Router } from '@angular/router';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';
import * as _moment from 'moment';
import { default as _rollupMoment } from 'moment';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
const moment = _rollupMoment || _moment;
export const DATE_FORMATS = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-registration-box',
  templateUrl: './registration-box.component.html',
  styleUrls: [
    './registration-box.component.scss',
    '../login-regist-common/login-regist-common.component.scss',
  ],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE],
    },
    { provide: MAT_DATE_FORMATS, useValue: DATE_FORMATS },
  ],
})
export class RegistrationBoxComponent implements AfterViewInit {
  constructor(public loginRegistCommonComponent: LoginRegistCommonComponent,
    public authenticationService: AuthenticationService,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router) { }

  public userRegisterDto: UserRegisterDto = {} as UserRegisterDto;
  public isPasswordHide = true;
  public isPasswordConfirmHide = true;
  public loading: boolean = false;

  public registrationForm: FormGroup = new FormGroup(
    {
      userFirstName: new FormControl('', [
        Validators.required,
        this.loginRegistCommonComponent.firstAndLastNameValidation,
      ]),
      userLastName: new FormControl('', [
        Validators.required,
        this.loginRegistCommonComponent.firstAndLastNameValidation,
      ]),
      userEmail: new FormControl(
        '',
        [
          Validators.required,
          Validators.pattern(
            '^([a-zA-Z0-9_-]+.)*[a-zA-Z0-9_-]+@[a-zA-Z0-9_]+(.[a-zA-Z0-9_-]+)*.[a-zA-Z]{1,6}$',
          ),
          this.loginRegistCommonComponent.noUnAllowedCharactersValidation,
        ],
        [this.loginRegistCommonComponent.userEmailValidator()],
      ),
      userPassword: new FormControl('', [
        Validators.required,
        this.loginRegistCommonComponent.minPasswordLenghtValidation,
        this.loginRegistCommonComponent.noWhitespaceValidation,
      ]),
      userPasswordConfirmation: new FormControl('', [Validators.required]),
      userBirthdate: new FormControl('', [
        Validators.required,
        this.loginRegistCommonComponent.birthDateValidation,
      ]),
    },
    { validators: this.loginRegistCommonComponent.passwordsMatch },
  );

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public ngAfterViewInit() {
    this.route.queryParams
      .subscribe(params => {
        if (params.email) {
          this.registrationForm.controls['userEmail'].setValue(params.email);
          this.registrationForm.controls['userEmail'].disable();
        }
      },
      );       
  }

  public onSubmit() {
    if (this.registrationForm.valid) {
      const dto: RegisterDto = {
        userRegisterDto: this.userRegisterDto,
        clientUrl: `${environment.clientUrl}/confirm-email`,
      };
      this.loading = true;

      this.authenticationService
        .register(dto)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
          () => {
            this.loading = false;
            this.router.navigate(['/successful-registration'], {
              queryParams: { email: this.userRegisterDto.email },
            });
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
                error.message,
                'Something went wrong',
              );
            }
          },
        );
    }
  }
}
