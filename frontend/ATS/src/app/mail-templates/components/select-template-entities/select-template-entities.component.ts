import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, Subject } from 'rxjs';
import { map, startWith, takeUntil } from 'rxjs/operators';
import { ProjectInfo } from 'src/app/projects/models/project-info';
import { ProjectService } from 'src/app/projects/services/projects.service';
import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { VacancyData } from 'src/app/shared/models/vacancy/vacancy-data';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { MailTemplateService } from 'src/app/shared/services/mail-template.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { VacancyDataService } from 'src/app/shared/services/vacancy-data.service';

@Component({
  selector: 'app-select-template-entities',
  templateUrl: './select-template-entities.component.html',
  styleUrls: ['./select-template-entities.component.scss'],
})
export class SelectTemplateEntitiesComponent implements OnInit {
  public validationGroup: FormGroup = new FormGroup({
    email: new FormControl('', [
      Validators.email,
      Validators.pattern('^\\S{1,}@\\S{3,}\\.[a-z]+'),
    ]),
  });

  email: string = '';
  id: string = '';
  buttonDisable: boolean = false;

  isVacancyRequired: boolean = false;
  isProjectRequired: boolean = false;
  isApplicantRequired: boolean = false;
  private readonly unsubscribe$: Subject<void> = new Subject<void>();
  projects: ProjectInfo[] = [];
  vacancies: VacancyData[] = [];
  applicants: Applicant[] = [];

  vacancyControl = new FormControl();
  vacancyButtonDisable: boolean = true;
  selectedVacancy!: VacancyData;
  filteredVacancies!: Observable<VacancyData[]>;

  projectControl = new FormControl();
  projectButtonDisable: boolean = true;
  selectedProject!: ProjectInfo;
  filteredProjects!: Observable<ProjectInfo[]>;

  applicantControl = new FormControl();
  applicantButtonDisable: boolean = true;
  selectedApplicant!: Applicant;
  filteredApplicants!: Observable<Applicant[]>;

  constructor(@Inject(MAT_DIALOG_DATA) public data:
  { id: string, isVacancyRequired: boolean, isProjectRequired: boolean,
    isApplicantRequired: boolean },
  private readonly applicantsService: ApplicantsService,
  private readonly vacanciesService: VacancyDataService,
  private readonly projectService: ProjectService,
  private mailTemplateService: MailTemplateService,
  private readonly notificationsService: NotificationService,
  private modal: MatDialogRef<SelectTemplateEntitiesComponent>,
  ) {
    this.id = data.id;
    this.isVacancyRequired = data.isVacancyRequired;
    this.isProjectRequired = data.isProjectRequired;
    this.isApplicantRequired = data.isApplicantRequired;
  }

  ngOnInit(): void {
    if (this.isVacancyRequired) {
      this.getVacancies();
    }

    if (this.isProjectRequired) {
      this.getProjects();
    }

    if (this.isApplicantRequired) {
      this.getApplicants();
    }

    this.vacancyControl.valueChanges.subscribe((vacancy) => {
      if (typeof vacancy !== 'string') {
        this.selectedVacancy = vacancy;
        this.vacancyButtonDisable = false;
      }
      else {
        this.vacancyButtonDisable = true;
      }
    });

    this.projectControl.valueChanges.subscribe((project) => {
      if (typeof project !== 'string') {
        this.selectedProject = project;
        this.projectButtonDisable = false;
      }
      else {
        this.projectButtonDisable = true;
      }
    });

    this.applicantControl.valueChanges.subscribe((appicant) => {
      if (typeof appicant !== 'string') {
        this.selectedApplicant = appicant;
        this.applicantButtonDisable = false;
      }
      else {
        this.applicantButtonDisable = true;
      }
    });
  }

  public getProjects() {
    this.projectService
      .getProjects()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        this.projects = resp.body!;

        this.filteredProjects = this.projectControl.valueChanges.pipe(
          startWith(''),
          map(value => typeof value === 'string' ? value : value.name),
          map(name => name ? this._projectFilter(name) : this.projects.slice()),
        );
      });
  }

  public getVacancies() {
    this.vacanciesService
      .getList()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        this.vacancies = resp;

        this.filteredVacancies = this.vacancyControl.valueChanges.pipe(
          startWith(''),
          map(value => typeof value === 'string' ? value : value.title),
          map(title => title ? this._vacancyFilter(title) : this.vacancies.slice()),
        );
      });
  }

  public getApplicants() {
    this.applicantsService
      .getApplicants()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        this.applicants = resp;

        this.filteredApplicants = this.applicantControl.valueChanges.pipe(
          startWith(''),
          map(value => typeof value === 'string' ? value : `${value.firstName} ${value.lastName}`),
          map(fullName => fullName ? this._applicantFilter(fullName) : this.applicants.slice()),
        );
      });
  }

  onSend() {
    this.buttonDisable = true;
    this.mailTemplateService.SendEmail(this.id, this.email, 
      this.selectedVacancy, this.selectedProject, this.selectedApplicant)
      .subscribe(_=>{
        this.modal.close();
        this.notificationsService.showSuccessMessage('Email sent');
      });
  }

  private _vacancyFilter(value: string): VacancyData[] {
    const filterValue = value?.toLowerCase();
    return this.vacancies.filter(option => option.title?.toLowerCase().includes(filterValue));
  }

  private _projectFilter(value: string): ProjectInfo[] {
    const filterValue = value?.toLowerCase();
    return this.projects.filter(option => option.name?.toLowerCase().includes(filterValue));
  }

  private _applicantFilter(value: string): Applicant[] {
    const filterValue = value?.toLowerCase();
    return this.applicants.filter(option => 
      `${option.firstName} ${option.lastName}`?.toLowerCase().includes(filterValue) || 
      option.firstName?.toLowerCase().includes(filterValue) || 
      option.lastName?.toLowerCase().includes(filterValue));
  }

  displayFnVacancy(vacancy: VacancyData): string {
    return vacancy && vacancy.title ?
      `${vacancy.title}` : '';
  }

  displayFnProject(project: ProjectInfo): string {
    return project && project.name ?
      `${project.name}` : '';
  }

  displayFnApplicant(applicant: Applicant): string {
    return applicant && applicant.firstName && applicant.lastName ?
      `${applicant.firstName} ${applicant.lastName}` : '';
  }
}
