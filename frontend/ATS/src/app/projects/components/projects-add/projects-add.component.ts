import { Component, OnDestroy, OnInit } from '@angular/core';
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
import { Tag } from 'src/app/shared/models/tags/tag';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-projects-add',
  templateUrl: './projects-add.component.html',
  styleUrls: ['./projects-add.component.scss'],
})
export class ProjectsAddComponent implements OnDestroy {
  technologies = new FormControl();
  technologiesList: string[] = [
    'ASP.NET',
    'WPF',
    'ADO.NET',
    'LINQ',
    'Angular',
    'React',
    'Vue.js',
    'JQuery',
  ];

  project: ProjectInfo = new ProjectInfo();

  urlRegEx: string = '(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?';

  projectCreateForm = new FormGroup({
    'name': new FormControl(this.project.name,
      [Validators.required,
        Validators.minLength(3),
        Validators.maxLength(15)]),
    'logo': new FormControl(this.project.logo,
      []),
    'description': new FormControl(this.project.description,
      [Validators.required,
        Validators.minLength(10)]),
    'teamInfo': new FormControl(this.project.description,
      [Validators.required,
        Validators.minLength(10)]),
    'websiteLink': new FormControl(this.project.websiteLink,
      [Validators.required,
        URLValidator()]),
    'tags':new FormControl(this.project.tags,[]),
  });

  public loading: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private projectService: ProjectService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<ProjectsAddComponent>,
  ) {
    this.dialogRef.disableClose = true;
    this.dialogRef.backdropClick().subscribe((_) => this.onFormClose());
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public onFormClose() {
    if (this.projectCreateForm.dirty) {
      if (confirm('Make sure you are saved everything. Continue?')) {
        this.dialogRef.close();
      }
    } else {
      this.dialogRef.close();
    }
  }

  public onSubmited() {
    this.project = this.projectCreateForm.value;
    this.loading = true;

    this.projectService
      .createProject(this.project)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        () => {
          this.loading = false;

          this.notificationService.showSuccessMessage(
            `Project ${this.project.name} created!`,
          );
        },
        (error) => {
          this.loading = false;
          this.notificationService.showErrorMessage(error.message);
        },
      );

    this.dialogRef.close();
  }
  
  public updateTags(tags: Tag[]): void {
    this.project.tags.tagDtos = tags;
  }
}

function URLValidator(): ValidatorFn {
  let emailRe: RegExp = new RegExp(
    '(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?',
  );
  return (control: AbstractControl): ValidationErrors | null => {
    const valid = emailRe.test(control.value);
    return valid ? null : { url: { value: control.value } };
  };
}
