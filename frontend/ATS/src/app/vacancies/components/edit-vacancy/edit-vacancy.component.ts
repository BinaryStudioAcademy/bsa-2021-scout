import {
  Component,
  EventEmitter,
  Inject,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Project } from 'src/app/shared/models/projects/project';
import { Stage } from 'src/app/shared/models/stages/stage';
import { VacancyCreate } from 'src/app/shared/models/vacancy/vacancy-create';
import { VacancyFull } from 'src/app/shared/models/vacancy/vacancy-full';
import { ProjectService } from 'src/app/shared/services/project.service';
import { VacancyService } from 'src/app/shared/services/vacancy.service';
import { MatChipInputEvent } from '@angular/material/chips';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { VacancyData } from 'src/app/shared/models/vacancy/vacancy-data';
import { Tag } from 'src/app/shared/models/tags/tag';
import { ElasticEntity } from 'src/app/shared/models/elastic-entity/elastic-entity';
import { ElasticType } from 'src/app/shared/models/elastic-entity/elastic-type';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-edit-vacancy',
  templateUrl: './edit-vacancy.component.html',
  styleUrls: ['./edit-vacancy.component.scss'],
})
export class EditVacancyComponent implements OnInit, OnDestroy {
  vacancyForm!: FormGroup;
  isOpenCreateStage: Boolean = false;
  submitted: Boolean = false;
  selectedProjects: Project[] = [];
  vacancy: VacancyCreate = {} as VacancyCreate;
  stageToEdit: Stage = {} as Stage;
  private projects: Project[] = [];
  isEditStageMode: Boolean = false;
  selectable = true;
  removable = true;
  addOnBlur = true;
  elasticEntity : ElasticEntity = {} as ElasticEntity; 
  vacancyId : string = '';
  tierFrom: number = 0;
  tierTo: number = 0;


  @Output() vacancyChange = new EventEmitter<VacancyFull>();

  public loading: boolean = true;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    public dialogRef: MatDialogRef<EditVacancyComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { vacancyToEdit: VacancyData },
    private fb: FormBuilder,
    public projectService: ProjectService,
    public vacancyService: VacancyService,
    public notificationService: NotificationService,
  ) {
    this.vacancyForm = this.fb.group(
      {
        title: ['', [Validators.required]],
        description: ['', [Validators.required]],
        requirements: ['', [Validators.required]],
        projectId: ['', [Validators.required]],
        salaryFrom: ['', [Validators.required]],
        salaryTo: ['', [Validators.required]],
        tierFrom: ['', [Validators.required]],
        tierTo: ['', [Validators.required]],
        link: ['', [Validators.required]],
        isHot: [''],
        isRemote: [''],
        tags: [''],
        stages: this.stageList,
      },
      { validator: this.customSalaryValidation },
    );
  }

  ngOnInit() {
    this.projectService
      .getByCompany()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (response) => {
          this.loading = false;
          this.projects = response;
          this.selectedProjects = this.projects;
        },
        () => {
          this.loading = false;
          this.notificationService.showErrorMessage('Failed to load projects.');
        },
      );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  customSalaryValidation(formGroup: FormGroup): any {
    let salaryFrom = formGroup.controls['salaryFrom'].value;
    let salaryTo = formGroup.controls['salaryTo'].value;
    let error = parseInt(salaryFrom, 10) > parseInt(salaryTo, 10);
    if (error) {
      formGroup.controls['salaryTo'].setErrors({ salaryRangeIsWrong: true });
    }
  }

  isTierFromLessTierTo(tierTo: Number): Boolean {
    let tierFrom = parseInt(this.vacancyForm.controls['tierFrom'].value, 10);
    if (tierFrom <= tierTo) {
      return true;
    }
    this.vacancyForm.controls['tierTo'].reset;
    return false;
  }

  ngOnInit() {
    if (this.data.vacancyToEdit) {
      this.vacancyService.getById(this.data.vacancyToEdit.id).subscribe(
        response => {
          console.log(response);
          if (!response.tags) {
            response.tags = {
              id: '',
              elasticType: 1,
              tagDtos: [],
            };
          }
          else{
            this.elasticEntity.id = response.tags.id;
          }
          this.vacancyForm.setValue({
            title: response.title,
            description: response.description,
            requirements: response.requirements,
            projectId: response.projectId,
            salaryFrom: response.salaryFrom,
            salaryTo: response.salaryTo,
            tierFrom: response.tierFrom.toString(),
            tierTo: response.tierTo.toString(),
            link: response.sources,
            isHot: response.isHot,
            isRemote: response.isRemote,
            tags: '',
            stages: response.stages,
          });
          response.stages.sort((a,b) => a.index - b.index);
          this.stageList = response.stages;
          this.tags = response.tags.tagDtos;
        });
    }
    this.projectService.getByCompany().subscribe(
      response => {
        this.projects = response;
        this.selectedProjects = this.projects;
      });
  }

  //------------------VACANCY------------------
  createVacancy() {
    this.submitted = true;
    this.loading = true;


    this.elasticEntity.tagDtos = this.tags;
    this.elasticEntity.elasticType = ElasticType.VacancyTags;
    this.vacancy = {
      title: this.vacancyForm.controls['title'].value,
      description: this.vacancyForm.controls['description'].value,
      requirements: this.vacancyForm.controls['requirements'].value,
      projectId: this.vacancyForm.controls['projectId'].value,
      salaryFrom: parseInt(this.vacancyForm.controls['salaryFrom'].value, 10),
      salaryTo: parseInt(this.vacancyForm.controls['salaryTo'].value, 10),
      tierFrom: parseInt(this.vacancyForm.controls['tierFrom'].value, 10),
      tierTo: parseInt(this.vacancyForm.controls['tierTo'].value, 10),
      sources: this.vacancyForm.controls['link'].value,
      isHot: this.vacancyForm.controls['isHot'].value ? true : false,
      isRemote: this.vacancyForm.controls['isRemote'].value ? true : false,
      tags : this.elasticEntity,
      stages: this.stageList,
    };
    if (!this.data.vacancyToEdit) {
      this.vacancyService
        .postVacancy(this.vacancy)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
          (response) => {
            this.loading = false;
            this.vacancyChange.emit(response);
          },
          () => {
            this.loading = false;
            this.notificationService.showErrorMessage('Failed to create vacancy.');
          },
        );
    } else {
      this.vacancyService
        .putVacancy(this.vacancy, this.data.vacancyToEdit.id)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
          (response) => {
            this.loading = false;
            this.vacancyChange.emit(response);
          },
          () => {
            this.loading = false;
            this.notificationService.showErrorMessage('Failed to update vacancy.');
          },
        );
    }


    this.dialogRef.close();
  }

  get vacancyFormControl() {
    return this.vacancyForm.controls;
  }

  //Tag field
  readonly separatorKeysCodes = [ENTER, COMMA] as const;
  tags: Tag[] = [
    { id: '1', tagName: 'Devops' },
    { id: '2', tagName: 'Ukraine' },
    { id: '3', tagName: 'Job offer' },
  ];

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (value) {
      this.tags.push({ tagName: value, id: this.tags.length.toString() }); //needs to be fixed
    }
    event.chipInput!.clear();
  }

  remove(tag: Tag): void {
    const index = this.tags.indexOf(tag);

    if (index >= 0) {
      this.tags.splice(index, 1);
    }
  }

  //Project field search
  onKey(event: Event) {
    this.selectedProjects = this.search((<HTMLInputElement>event.target).value);
  }

  search(value: string) {
    let filter = value.toLowerCase();
    return this.projects.filter((option) =>
      option.name.toLowerCase().startsWith(filter),
    );
  }

  stageList: Stage[] = [
    {
      id: '',
      name: 'Contacted',
      index: 0,
      type: 0,
      actions: [{
        id : '',
        name: 'Schedule interview action',
        actionType: 3,
        stageId: '',
      }],
      rates: 'English',
      IsReviewable: true,
      vacancyId: '',
    },
    {
      id: '',
      name: 'Hr interview',
      index: 1,
      type: 1,
      actions: [{
        id : '',
        name: 'Schedule interview action',
        actionType: 3,
        stageId: '',
      }],
      rates: 'English',
      IsReviewable: true,
      vacancyId: '',
    },
    {
      id: '',
      name: 'Tech interview',
      index: 2,
      type: 2,
      actions: [{
        id : '',
        name: 'Schedule interview action',
        actionType: 3,
        stageId: '',
      }],
      rates: 'English',
      IsReviewable: true,
      vacancyId: '',
    },
    {
      id: '',
      name: 'Live coding session',
      index: 3,
      type: 3,
      actions: [{
        id : '',
        name: 'Schedule interview action',
        actionType: 3,
        stageId: '',
      }],
      rates: 'English',
      IsReviewable: true,
      vacancyId: '',
    },
    {
      id: '',
      name: 'Pre-offer',
      index: 4,
      type: 4,
      actions: [{
        id : '',
        name: 'Schedule interview action',
        actionType: 3,
        stageId: '',
      }],
      rates: 'English',
      IsReviewable: true,
      vacancyId: '',
    },
    {
      id: '',
      name: 'Offer',
      index: 5,
      type: 5,
      actions: [{
        id : '',
        name: 'Schedule interview action',
        actionType: 3,
        stageId: '',
      }],
      rates: 'English',
      IsReviewable: true,
      vacancyId: '',
    },
  ];

  ///-----------------STAGES-----------------
  onEditStage(stageToEdit: Stage) {
    this.stageToEdit = stageToEdit;
    this.isOpenCreateStage = true;
    this.isEditStageMode = true;
  }

  //changes indexes of stages
  sortStageList() {
    let index = 0;
    this.stageList.forEach(x => {
      x.index = index;
      index++;
    });
    return this.stageList;
  }

  //common func for saving
  toSave(newStage: Stage) {
    newStage.vacancyId = this.vacancyId;
    if (this.isEditStageMode) {
      let stage = this.stageList.find(x => x.index === newStage.index);
      if (stage?.index) {
        newStage.id = stage.id;
        this.stageList[stage?.index - 1] = newStage;
      }
      this.isEditStageMode = false;
    } else {
      newStage.index = this.stageList.length + 1;
      this.stageList.push(newStage);
    }
    this.stageToEdit = {} as Stage;
  }

  saveStage(newStage: Stage) {
    this.toSave(newStage);
    this.displayCreateStage();
  }

  saveStageAndAdd(newStage: Stage) {
    this.toSave(newStage);
  }

  cancelStageEdit() {
    this.stageToEdit = {} as Stage;
    this.displayCreateStage();
  }

  onDeleteStage(selectedStage: Stage) {
    let id = this.stageList.findIndex((a) => a.index == selectedStage.index);
    this.stageList.splice(id, 1);
  }

  displayCreateStage() {
    this.isOpenCreateStage = !this.isOpenCreateStage;
  }

  //moving stages
  dropStage(event: CdkDragDrop<any>) {
    this.stageList[event.previousContainer.data.index] =
      event.container.data.item;
    this.stageList[event.container.data.index] =
      event.previousContainer.data.item;
  }
}
