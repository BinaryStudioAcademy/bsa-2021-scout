import { Component, Inject, OnInit } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  ValidatorFn,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { ProjectInfo } from 'src/app/projects/models/project-info';
import { ProjectService } from 'src/app/projects/services/projects.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-projects-edit',
  templateUrl: './projects-edit.component.html',
  styleUrls: ['./projects-edit.component.scss'],
})
export class ProjectsEditComponent {

  technologies = new FormControl();
  technologiesList: string[] = ['ASP.NET', 'WPF', 'ADO.NET', 'LINQ',
    'Angular', 'React', 'Vue.js', 'JQuery'];

  project: ProjectInfo = new ProjectInfo();

  projectCreateForm = new FormGroup({
    'name': new FormControl(this.project.name,
      [Validators.required,
        Validators.minLength(3),
        Validators.maxLength(15)]),
    'logo': new FormControl(this.project.logo,
      [Validators.required]),
    'description': new FormControl(this.project.description,
      [Validators.required,
        Validators.minLength(10)]),
    'teamInfo': new FormControl(this.project.description,
      [Validators.required,
        Validators.minLength(10)]),
    'websiteLink': new FormControl(this.project.websiteLink,
      [Validators.required,
        URLValidator()]),
  });



  constructor(
    private projectService: ProjectService,
    private dialogRef: MatDialogRef<ProjectsEditComponent>,
    private notificationService: NotificationService,
    @Inject(MAT_DIALOG_DATA) public data: { project: ProjectInfo },
  ) {
    this.dialogRef.disableClose = true;
    this.dialogRef.backdropClick().subscribe(_ =>
      this.onFormClose());

    this.project = data.project;

    this.projectCreateForm.setValue({
      'name': this.project.name,
      'logo': this.project.logo,
      'description': this.project.description,
      'teamInfo': this.project.teamInfo,
      'websiteLink': this.project.websiteLink,
    });
  }

  public onFormClose() {
    if (this.projectCreateForm.dirty) {
      if (confirm('Make sure you are saved everything. Continue?')) {
        this.dialogRef.close();
      }
    }
    else {
      this.dialogRef.close();
    }
  }

  public onSubmited() {
    this.project.name = this.projectCreateForm.value.name;
    this.project.logo = this.projectCreateForm.value.logo;
    this.project.description = this.projectCreateForm.value.description;
    this.project.teamInfo = this.projectCreateForm.value.teamInfo;
    this.project.websiteLink = this.projectCreateForm.value.websiteLink;


    this.projectService.updateProject(this.project)
      .subscribe(_ => {
        this.notificationService.showSuccessMessage(`Project ${this.project.name} updated!`);
      },
      (error) => (this.notificationService.showErrorMessage(error.message)));
  }

}

function URLValidator(): ValidatorFn {
  let emailRe: RegExp = new RegExp('(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?');
  return (control: AbstractControl): ValidationErrors | null => {
    const valid = emailRe.test(control.value);
    return valid ? null : { url: { value: control.value } };
  };
}
 