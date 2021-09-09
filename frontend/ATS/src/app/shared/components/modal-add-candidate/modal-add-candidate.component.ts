import { Component, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith, takeUntil } from 'rxjs/operators';
import { VacancyDataService } from 'src/app/shared/services/vacancy-data.service';
import { MarkedApplicant } from 'src/app/shared/models/applicants/marked-applicant';
import { ShortVacancyWithDepartment }
  from 'src/app/shared/models/vacancy/short-vacancy-with-department';
import { VacancyCandidateService } from 'src/app/shared/services/vacancy-candidate.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { GetShortApplicant } from '../../models/applicants/get-short-applicant';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-modal-add-candidate',
  templateUrl: './modal-add-candidate.component.html',
  styleUrls: ['./modal-add-candidate.component.scss'],
})
export class AddCandidateModalComponent implements OnDestroy {
  public vacancyId: string = '';
  public disableVacanciesForm: boolean = true;
  public disableAddButton: boolean=true;

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
    @Inject(MAT_DIALOG_DATA) public data:
    { vacancyId: string, applicantId: string, applicantsIds: string[] },
    private readonly applicantsService: ApplicantsService,
    private readonly vacancyService: VacancyDataService,
    private readonly vacancyCandidateService: VacancyCandidateService,
    private notificationService: NotificationService,
    private modal: MatDialogRef<AddCandidateModalComponent>) {

    this.applicantsForm.disable();
    this.applicantsIds = data.applicantsIds;

    if (data.vacancyId) {
      this.vacancyId = data.vacancyId;

      this.vacancyService
        .getVacancy(this.vacancyId)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
          (value) => (this.selectedVacancy = value),
          (error) => this.OnError(error),
        );

      this.applicantsForm.enable();
      this.applicantsForm.valueChanges.subscribe(_=>{
        if(this.applicantsForm.value.length==0){
          this.disableAddButton=true;
        } else {
          this.disableAddButton=false;
        }
      });

      this.GetApplicants();
    } else if (data.applicantId) {
      this.disableVacanciesForm = false;

      this.applicantsService
        .getApplicantByCompany(data.applicantId)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
          (value) => {
            this.loading = false;
            this.selectedApplicant = value;
            let applicant: MarkedApplicant = new MarkedApplicant(
              this.selectedApplicant,
            );
            this.filteredApplicants.push(applicant);
            this.applicantsForm.setValue([applicant]);
          },
          (error) => this.OnError(error),
        );

      this.vacancyService.getNotAppliedVacanciesByApplicant(data.applicantId)
        .subscribe(vacancies => {
          this.vacancies = vacancies;
          if (vacancies.length == 0) {
            this.OnError(new Error('No vacancies for this applicant'));
          }
          this.filteredVacancies = this.vacanciesForm.valueChanges.pipe(
            startWith(''),
            map(value => typeof value === 'string' ? value : value.title),
            map(title => title ? this._filter(title) : this.vacancies.slice()),
          );
        },
        (error) => (this.OnError(error)));
        
      this.vacanciesForm.valueChanges.subscribe((vacancy) => {
        if (typeof vacancy !== 'string') {
          this.selectedVacancy = vacancy;
          this.vacancyId = this.selectedVacancy.id;
          this.disableAddButton=false;
        } else {
          this.disableAddButton=true;
        }
      },
      (error) => (this.OnError(error)));

    } else {
      this.disableVacanciesForm = false;
      this.applicantsForm.enable();

      this.vacancyService
        .getVacancies()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
          vacancies => {
            this.loading = false;
            this.vacancies = vacancies;
            this.filteredVacancies = this.vacanciesForm.valueChanges.pipe(
              startWith(''),
              map(value => typeof value === 'string' ? value : value.title),
              map(title => title ? this._filter(title) : this.vacancies.slice()),
            );
          },
          (error) => this.OnError(error),
        );

      this.vacanciesForm.valueChanges.subscribe((vacancy) => {
        if (typeof vacancy !== 'string') {
          this.disableAddButton = false;
          this.selectedVacancy = vacancy;
          this.vacancyId = this.selectedVacancy.id;

          this.GetApplicants();
        }
        else {
          this.disableAddButton = true;
        }
      },
      (error) => (this.OnError(error)));

      this.applicantsForm.valueChanges.subscribe((applicants) => {
        if(applicants.length == 0)
        {
          this.disableAddButton = true;
        }
        else
        {
          this.disableAddButton = false;
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
      .subscribe(
        value => {
          this.loading = false;
          this.filteredApplicants = value;

          let newApplicants: MarkedApplicant[] = [];

          this.applicantsIds?.forEach((applicantId) => {
            this.filteredApplicants.forEach((applicant) => {
              if (applicantId == applicant.id) {
                newApplicants.push(applicant);
              }
            });
          });

          if(this.applicantsIds) {
            this.filteredApplicants = newApplicants;
          }
          this.applicantsForm.patchValue(newApplicants.filter(applicant => !applicant.isApplied));

          if(newApplicants.length == 0)
          {
            this.disableAddButton = true;
          }
        },
        (error) => this.OnError(error),
      );
  }

  public OnCreate() {
    let markedApplicants: MarkedApplicant[] = this.applicantsForm.value;

    this.vacancyCandidateService.postRangeOfCandidates(this.vacancyId,
      markedApplicants.map(applicant => applicant.id))
      .subscribe(_ => {
        this.notificationService.showSuccessMessage('Successfully added');
        this.modal.close();
      }
      , (error) => {
        this.OnError(error);
        this.modal.close();
      });

    
  }

  private _filter(title: string): ShortVacancyWithDepartment[] {
    const filterValue = title.toLowerCase();

    return this.vacancies.filter((option) =>
      option.title.toLowerCase().includes(filterValue),
    );
  }

  displayFn(vacancy: ShortVacancyWithDepartment): string {
    return vacancy && vacancy.title && vacancy.department ?
      `${vacancy.title} / ${vacancy.department}` : '';
  }

  public onFormClose() {
    this.modal.close();
  }

  public OnError(error: Error) {
    this.loading = false;
    this.notificationService.showErrorMessage(error?.message ? 
      error.message : 'Apply vacancy failed');
    this.modal.close();
  }
}
