import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { IOption } from 'src/app/shared/components/multiselect/multiselect.component';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { VacancyData } from 'src/app/shared/models/vacancy/vacancy-data';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { VacancyDataService } from 'src/app/shared/services/vacancy-data.service';
import { UserTableData } from 'src/app/users/models/user-table-data';
import { UserDataService } from 'src/app/users/services/user-data.service';
import { CreateInterviewDto } from '../../models/create-interview-dto.model';
import { InterviewType } from '../../models/enums/interview-type.enum';
import { MeetingSource } from '../../models/enums/meeting-source.enum';
import { InterviewsService } from '../../services/interviews.service';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';
import * as _moment from 'moment';
import { default as _rollupMoment } from 'moment';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
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
  selector: 'app-create-interview',
  templateUrl: './create-interview.component.html',
  styleUrls: ['./create-interview.component.scss'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE],
    },
    { provide: MAT_DATE_FORMATS, useValue: DATE_FORMATS },
  ],
})
export class CreateInterviewComponent implements OnDestroy {

  interviewTypes: { name: string; type: InterviewType }[] = [
    {
      name: 'Behavioural',
      type: InterviewType.Behavioural,
    },
    {
      name: 'Interview',
      type: InterviewType.Interview,
    },
    {
      name: 'Meeting',
      type: InterviewType.Meeting,
    },
    {
      name: 'OnSite',
      type: InterviewType.OnSite,
    },
    {
      name: 'Technical',
      type: InterviewType.Technical,
    },
  ];
  meetingSources: { name: string; source: MeetingSource }[] = [
    {
      name: 'None',
      source: MeetingSource.GoogleMeet,
    },
    {
      name: 'GoogleMeet',
      source: MeetingSource.GoogleMeet,
    },
    {
      name: 'MSTeams',
      source: MeetingSource.MSTeams,
    },
    {
      name: 'Skype',
      source: MeetingSource.Skype,
    },
    {
      name: 'Slack',
      source: MeetingSource.Slack,
    },
    {
      name: 'Zoom',
      source: MeetingSource.Zoom,
    },
  ];
  editing : boolean = false;
  inteviewTime: string = '';
  inteviewDate: Date = new Date;
  interview: CreateInterviewDto = {} as CreateInterviewDto;
  users: UserTableData[] = [];
  applicants: Applicant[] = [];
  vacancies: VacancyData[] = [];
  usersOptions: IOption[] = [];
  urlRegEx: string = '(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?';
  interviewCreateForm: FormGroup = {} as FormGroup;
  public loading: boolean = false;
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private interviewsService: InterviewsService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<CreateInterviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { interview: CreateInterviewDto },
    private usersService: UserDataService,
    private applicantsService: ApplicantsService,
    private vacancyService: VacancyDataService,
  ) {
    this.interview = {} as CreateInterviewDto;
    this.interview.meetingSource = MeetingSource.None;
    if (data?.interview) {
      this.editing = true;
      this.interview = data.interview;
    }
    this.inteviewDate = new Date(this.interview.scheduled);
    if(this.interview.scheduled != undefined){
      this.inteviewTime = ('0' + (this.inteviewDate.getHours())).slice(-2) +
      ':' + ('0' + (this.inteviewDate.getMinutes())).slice(-2);
    }
    
    this.interviewCreateForm = new FormGroup({
      'meetingSource': new FormControl(this.interview.meetingSource),
      'title': new FormControl(this.interview.title,
        [Validators.required,
          Validators.minLength(3),
          Validators.maxLength(100)]),
      'interviewers': new FormControl(this.interview.userParticipantDatas,
        [Validators.required]),
      'candidateId': new FormControl(this.interview.candidateId,
        [Validators.required]),
      'vacancyId': new FormControl(this.interview.vacancyId,
        [Validators.required]),
      'interviewType': new FormControl(this.interview.interviewType,
        [Validators.required]),
      'date': new FormControl(this.interview.scheduled,
        [Validators.required,
          this.dateValidation]),
      'time': new FormControl(this.inteviewTime,
        [Validators.required]),
      'duration': new FormControl(this.interview.duration,
        [Validators.required,
          Validators.min(10),
          Validators.max(200)]),
      'meetingLink': new FormControl(this.interview.meetingLink,
        [URLValidator()]),
      'note': new FormControl(this.interview.note),
    });
    this.dialogRef.disableClose = true;
    this.dialogRef.backdropClick().subscribe((_) => this.onFormClose());
    this.usersService.getAllUsers()
      .pipe(
        takeUntil(this.unsubscribe$),
      )
      .subscribe((resp) => {
        this.users = resp;
        this.usersOptions = this.usersToOptions(this.users);
      });
    this.applicantsService.getApplicants()
      .pipe(
        takeUntil(this.unsubscribe$),
      )
      .subscribe((resp) => {
        this.applicants = resp;
      });
    this.vacancyService.getList()
      .pipe(
        takeUntil(this.unsubscribe$),
      )
      .subscribe((resp) => {
        this.vacancies = resp;
      });
  }

  public usersToOptions(users?: UserTableData[]): IOption[] {
    return (users ?? []).map((u) => ({
      id: u.id as string,
      value: u.id as string,
      label: u.firstName + ' ' + u.lastName,
    }));
  }

  public usersToActions(options?: IOption[], stageChangeEventType: number = 0): UserTableData[] {
    if (stageChangeEventType == 1) {
      return (options ?? []).map(
        ({ id }) => this.users.find((u) => u.id === id)!,
      );
    }
    else {
      return (options ?? []).map(
        ({ id }) => this.users.find((u) => u.id === id)!,
      );
    }
  }


  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public onFormClose() {
    this.dialogRef.close();
  }

  selectTime(time : string){
    this.inteviewTime  = time;
  }

  public onSubmited() {
    this.interview = this.interviewCreateForm.value;
    this.interview.isReviewed = true;
    this.loading = true;
    this.inteviewDate = new Date(this.interviewCreateForm.controls['date'].value);

    var interviewers = (this.interviewCreateForm.controls['interviewers'].value as UserTableData[]);
    this.interview.userParticipants = [];
    interviewers.forEach((element: UserTableData) => {
      this.interview.userParticipants.push(element.id as string);
    });
    var newDate = new Date(this.inteviewDate.getFullYear() +
      '-' + ('0' + (this.inteviewDate.getMonth() + 1)).slice(-2) +
      '-' + ('0' + (this.inteviewDate.getDate())).slice(-2)
      + 'T' + this.inteviewTime + '+0000');
    this.interview.scheduled = newDate.toISOString();
    if(this.data){
      this.interview.id = this.data.interview.id;
      this.interviewsService
        .updateInterview(this.interview)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
          () => {
            this.loading = false;
            this.notificationService.showSuccessMessage(
              `Interview ${this.interview.title} updated!`,
            );
          },
          (error) => {
            this.loading = false;
            this.notificationService.showErrorMessage(error.message);
          },
        );
    }
    else{
      this.interviewsService
        .createInterview(this.interview)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
          () => {
            this.loading = false;
            this.notificationService.showSuccessMessage(
              `Interview ${this.interview.title} created!`,
            );
          },
          (error) => {
            this.loading = false;
            this.notificationService.showErrorMessage(error.message);
          },
        );
    }
    

    this.dialogRef.close();
  }

  public dateValidation(control: FormControl) {
    if (control.value != null) {
      let enterdDate: Date = new Date(control.value);
      return moment(enterdDate) > moment(new Date()).subtract(1, 'day') &&
        moment(enterdDate) < moment(new Date()).add(1, 'year')
        ? null
        : { datevalidate: true };
    }
    return null;
  }

}

function URLValidator(): ValidatorFn {
  let emailRe: RegExp = new RegExp(
    '(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?',
  );
  return (control: AbstractControl): ValidationErrors | null => {
    const isLenghtNotZero = control.value?.length > 0;
    const valid = emailRe.test(control.value);
    return valid || !isLenghtNotZero ? null : { url: { value: control.value } };
  };
}



