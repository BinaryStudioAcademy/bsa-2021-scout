import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  ValidatorFn,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ProjectInfo } from 'src/app/projects/models/project-info';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ProjectService } from 'src/app/projects/services/projects.service';

@Component({
  selector: 'app-projects-add',
  templateUrl: './projects-add.component.html',
  styleUrls: ['./projects-add.component.scss'],
})
export class ProjectsAddComponent {
  technologies = new FormControl();
  technologiesList: string[] = ['ASP.NET', 'WPF', 'ADO.NET', 'LINQ',
    'Angular', 'React', 'Vue.js', 'JQuery'];

  project: ProjectInfo = new ProjectInfo();

  urlRegEx: string =
  '(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?';

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
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<ProjectsAddComponent>,
  ) {
    this.dialogRef.disableClose = true;
    this.dialogRef.backdropClick().subscribe(_ =>
      this.onFormClose());
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
    this.project = this.projectCreateForm.value;

    this.projectService.createProject(this.project)
      .subscribe(_ => {
        this.notificationService.showSuccessMessage(`Project ${this.project.name} created!`);
      },
      (error) => (this.notificationService.showErrorMessage(error.message)));

    this.dialogRef.close();
  }

}

function URLValidator(): ValidatorFn {
  let emailRe: RegExp = new RegExp('(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?');
  return (control: AbstractControl): ValidationErrors | null => {
    const valid = emailRe.test(control.value);
    return valid ? null : { url: { value: control.value } };
  };
}
