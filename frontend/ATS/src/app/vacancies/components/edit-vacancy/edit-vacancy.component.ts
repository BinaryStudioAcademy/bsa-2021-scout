import {Component,EventEmitter,Inject,OnChanges,OnInit,Output,SimpleChanges} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import { Project } from 'src/app/shared/models/projects/project';
import { Stage } from 'src/app/shared/models/stages/stage';
import { VacancyCreate } from 'src/app/shared/models/vacancy/vacancy-create';
import { VacancyFull } from 'src/app/shared/models/vacancy/vacancy-full';
import { ProjectService } from 'src/app/shared/services/project.service';
import { VacancyService } from 'src/app/shared/services/vacancy.service';
import { MatChipInputEvent } from '@angular/material/chips';
import { CdkDragDrop} from '@angular/cdk/drag-drop';
import { VacancyData } from 'src/app/shared/models/vacancy/vacancy-data';
import { Tag } from 'src/app/shared/models/tags/tag';

@Component({
  selector: 'app-edit-vacancy',
  templateUrl: './edit-vacancy.component.html',
  styleUrls: ['./edit-vacancy.component.scss'],
})
export class EditVacancyComponent implements OnInit {

  vacancyForm!: FormGroup;
  isOpenCreateStage : Boolean = false;
  submitted:Boolean = false;
  selectedProjects:Project[] = []; 
  vacancy:VacancyCreate = {} as VacancyCreate;
  stageToEdit:Stage = {} as Stage;
  private projects: Project[] = [];
  isEditStageMode:Boolean = false;
  selectable = true;
  removable = true;
  addOnBlur = true;

  @Output() vacancyChange = new EventEmitter<VacancyFull>();

  constructor(
    public dialogRef: MatDialogRef<EditVacancyComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {vacancyToEdit: VacancyData},
    private fb: FormBuilder,
    public projectService: ProjectService,
    public vacancyService:VacancyService) {
    this.vacancyForm = this.fb.group({
      title: ['', [Validators.required]],
      description: ['', [Validators.required]],
      requirements: ['',[Validators.required]],
      projectId:['', [Validators.required]],
      salaryFrom:['', [Validators.required]],
      salaryTo:['', [Validators.required]],
      tierFrom:['', [Validators.required]],
      tierTo:['', [Validators.required]],
      link:['', [Validators.required]],
      isHot:[''],
      isRemote:[''],
      tags:[''],
      stages:this.stageList,
    }, {validator: this.customSalaryValidation},
    );
  }

  customSalaryValidation(formGroup: FormGroup): any {
    let salaryFrom = formGroup.controls['salaryFrom'].value;
    let salaryTo = formGroup.controls['salaryTo'].value;
    let error = (parseInt(salaryFrom,10) > parseInt(salaryTo,10));
    if(error){
      formGroup.controls['salaryTo'].setErrors({ salaryRangeIsWrong: true });
    }
  }

  isTierFromLessTierTo(tierTo:Number):Boolean{
    let tierFrom = parseInt(this.vacancyForm.controls['tierFrom'].value,10);
    if(tierFrom <= tierTo){
      return true;
    }
    this.vacancyForm.controls['tierTo'].reset;
    return false;
  }

  ngOnInit(){
    this.projectService.getByCompany().subscribe(
      response=>{
        this.projects = response;
        this.selectedProjects = this.projects;
      });
  }

  //------------------VACANCY------------------
  createVacancy(){
    this.submitted = true;
    this.vacancy = {
      title:this.vacancyForm.controls['title'].value,
      description:this.vacancyForm.controls['description'].value,
      requirements:this.vacancyForm.controls['requirements'].value,
      projectId:this.vacancyForm.controls['projectId'].value,
      salaryFrom:parseInt(this.vacancyForm.controls['salaryFrom'].value,10),
      salaryTo:parseInt(this.vacancyForm.controls['salaryTo'].value,10),
      tierFrom:parseInt(this.vacancyForm.controls['tierFrom'].value,10),
      tierTo:parseInt(this.vacancyForm.controls['tierTo'].value,10),
      sources:this.vacancyForm.controls['link'].value,
      isHot:this.vacancyForm.controls['isHot'].value ? true : false,
      isRemote:this.vacancyForm.controls['isRemote'].value ? true : false,
      tags: this.tags,
      stages: this.stageList,
    };

    if(!this.data.vacancyToEdit){
      this.vacancyService.postVacancy(this.vacancy)
        .subscribe(
          response=> this.vacancyChange.emit(response),
        );
    }else{
      
      this.vacancyService.putVacancy(this.vacancy, this.data.vacancyToEdit.id)
        .subscribe(
          response=> this.vacancyChange.emit(response),
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
    {id:'1', tagName: 'Devops'},
    {id:'2',tagName: 'Ukraine'},
    {id:'3',tagName: 'Job offer'},
  ];

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (value) {
      this.tags.push({tagName: value, id:this.tags.length.toString()}); //needs to be fixed
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
  onKey(event:Event) { 
    this.selectedProjects = this.search((<HTMLInputElement>event.target).value);
  }

  search(value: string) { 
    let filter = value.toLowerCase();
    return this.projects.filter(option => option.name.toLowerCase().startsWith(filter));
  }

  stageList:Stage[]=[
    {
      id:'aaaa',
      name: 'Test',
      index:2,
      action:'Prepare questions for interview',
      rates:['English'],
      isReviewRequired:true,
      vacancyId:'1',
    },
    {
      id:'bbbb',
      name: 'Interview',
      index:1,
      action:'Prepare questions for interview',
      rates:['English'],
      isReviewRequired:true,
      vacancyId:'2',
    },
    {id:'ccccc',
      name: 'Technical test',
      index:3,
      action:'Prepare questions for interview',
      rates:['English'],
      isReviewRequired:true,
      vacancyId:'1',
    },
    {
      id:'bbbb',
      name: '2nd interview',
      index:4,
      action:'Prepare questions for interview',
      rates:['English'],
      isReviewRequired:true,
      vacancyId:'2',
    },
    {id:'ddddd',
      name: 'Researching',
      index:5,
      action:'Prepare questions for interview',
      rates:['English'],
      isReviewRequired:true,
      vacancyId:'1',
    }
  ]





  ///-----------------STAGES-----------------
  onEditStage(stageToEdit: Stage){
    this.stageToEdit = stageToEdit;
    this.isOpenCreateStage = true;
    this.isEditStageMode = true;
  }

  //changes indexes of stages
  sortStageList(){
    let index = 1;
    this.stageList.forEach(x=>{
      x.index = index;
      index++;
    });
    console.log(this.stageList);
    return this.stageList;
  }

  //common func for saving
  toSave(newStage:Stage){
    if(this.isEditStageMode){
      let stageIndex = this.stageList.find(x=>x.index === newStage.index)?.index;
      if(stageIndex){
        this.stageList[stageIndex-1] = newStage;
      }
      this.isEditStageMode = false;
    }else{
      newStage.index = this.stageList.length + 1;
      this.stageList.push(newStage);
    }
    this.stageToEdit = {} as Stage;
  }

  saveStage(newStage:Stage){
    this.toSave(newStage);
    this.displayCreateStage();
  }

  saveStageAndAdd(newStage:Stage){
    this.toSave(newStage);
  }

  cancelStageEdit(){
    this.stageToEdit = {} as Stage;
    this.displayCreateStage();
  }

  onDeleteStage(selectedStage:Stage){
    let id  = this.stageList.findIndex((a)=>a.index == selectedStage.index);
    this.stageList.splice(id, 1);
    console.log(this.stageList);
  }

  displayCreateStage(){
    this.isOpenCreateStage = !this.isOpenCreateStage;
  }

  //moving stages
  dropStage(event: CdkDragDrop<any>) {
    this.stageList[event.previousContainer.data.index]=event.container.data.item;
    this.stageList[event.container.data.index]=event.previousContainer.data.item;
  }
}
