import { Component, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { map, startWith, takeUntil } from 'rxjs/operators';
import { VacancyDataService } from 'src/app/shared/services/vacancy-data.service';
import { MarkedApplicant } from 'src/app/shared/models/applicant/marked-applicant';

import {
  ShortVacancyWithDepartment,
} from 'src/app/shared/models/vacancy/short-vacancy-with-department';

import { VacancyCandidateService } from 'src/app/shared/services/vacancy-candidate.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { GetShortApplicant } from '../../models/applicant/get-short-applicant';

@Component({
  selector: 'app-modal-add-candidate',
  templateUrl: './modal-add-candidate.component.html',
  styleUrls: ['./modal-add-candidate.component.scss'],
})
export class AddCandidateModalComponent implements OnDestroy {
  public vacancyId: string = '';
  public disableApplicantsForm: boolean = true;
  public disableVacanciesForm: boolean = true;

  applicantsForm = new FormControl();
  filteredApplicants: MarkedApplicant[] = [];
  applicantsIds: string[] = [];
  selectedApplicant: GetShortApplicant = new GetShortApplicant();

  vacanciesForm = new FormControl();
  filteredVacancies!: Observable<ShortVacancyWithDepartment[]>;
  vacancies!: ShortVacancyWithDepartment[];
  selectedVacancy: ShortVacancyWithDepartment =
  new ShortVacancyWithDepartment();

  public loading: boolean = true;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    @Inject(MAT_DIALOG_DATA)
    public data: {
      vacancyId: string;
      applicantId: string;
      applicantsIds: string[];
    },
    private readonly applicantsService: ApplicantsService,
    private readonly vacancyService: VacancyDataService,
    private readonly vacancyCandidateService: VacancyCandidateService,
    private notificationService: NotificationService,
    private modal: MatDialogRef<AddCandidateModalComponent>,
  ) {
    this.applicantsIds = data.applicantsIds;

    if (data.vacancyId) {
      this.vacancyId = data.vacancyId;

      this.vacancyService
        .getVacancy(this.vacancyId)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe((value) => {
          this.loading = false;
          this.selectedVacancy = value;
        });

      this.disableApplicantsForm = false;

      this.GetApplicants();
    } else if (data.applicantId) {
      this.disableVacanciesForm = false;
      let oneCompleted: boolean = false;

      this.applicantsService
        .getApplicantByCompany(data.applicantId)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe((value) => {
          if (!oneCompleted) {
            oneCompleted = true;
          } else {
            this.loading = false;
          }

          this.selectedApplicant = value;
          let applicant: MarkedApplicant = new MarkedApplicant(
            this.selectedApplicant,
          );
          this.filteredApplicants.push(applicant);
          this.applicantsForm.setValue([applicant]);
        });

      this.vacancyService
        .getNotAppliedVacanciesByApplicant(data.applicantId)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe((vacancies) => {
          if (!oneCompleted) {
            oneCompleted = true;
          } else {
            this.loading = false;
          }

          this.vacancies = vacancies;

          this.filteredVacancies = this.vacanciesForm.valueChanges.pipe(
            startWith(''),
            map((value) => (typeof value === 'string' ? value : value.title)),
            map((title) =>
              title ? this._filter(title) : this.vacancies.slice(),
            ),
          );
        });

      this.vacanciesForm.valueChanges.subscribe((vacancy) => {
        if (typeof vacancy !== 'string') {
          this.selectedVacancy = vacancy;
          this.vacancyId = this.selectedVacancy.id;
        }
      });
    } else {
      this.disableVacanciesForm = false;

      this.vacancyService
        .getVacancies()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe((vacancies) => {
          this.vacancies = vacancies;
          this.loading = false;

          this.filteredVacancies = this.vacanciesForm.valueChanges.pipe(
            startWith(''),
            map((value) => (typeof value === 'string' ? value : value.title)),
            map((title) =>
              title ? this._filter(title) : this.vacancies.slice(),
            ),
          );
        });

      this.vacanciesForm.valueChanges.subscribe((vacancy) => {
        if (typeof vacancy !== 'string') {
          this.disableApplicantsForm = false;

          this.selectedVacancy = vacancy;
          this.vacancyId = this.selectedVacancy.id;

          this.GetApplicants();
        } else {
          this.disableApplicantsForm = true;
        }
      });
    }
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public GetApplicants() {
    this.loading = true;

    this.applicantsService
      .getMarkedApplicants(this.vacancyId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((value) => {
        this.loading = false;
        this.filteredApplicants = value;

        this.applicantsIds?.forEach((applicantId) => {
          this.filteredApplicants.forEach((applicant) => {
            if (applicantId == applicant.id && !applicant.isApplied) {
              this.applicantsForm.setValue([applicant]);
            }
          });
        });
      });
  }

  public OnCreate() {
    let markedApplicants: MarkedApplicant[] = this.applicantsForm.value;
    this.loading = true;

    this.vacancyCandidateService
      .postRangeOfCandidates(
        this.vacancyId,
        markedApplicants.map((applicant) => applicant.id),
      )
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((_) => {
        this.loading = false;
        this.notificationService.showSuccessMessage('Successfully added');
      });

    this.modal.close();
  }

  private _filter(title: string): ShortVacancyWithDepartment[] {
    const filterValue = title.toLowerCase();

    return this.vacancies.filter((option) =>
      option.title.toLowerCase().includes(filterValue),
    );
  }

  displayFn(vacancy: ShortVacancyWithDepartment): string {
    return vacancy && vacancy.title && vacancy.department
      ? `${vacancy.title} / ${vacancy.department}`
      : '';
  }

  public onFormClose() {
    this.modal.close();
  }
}
