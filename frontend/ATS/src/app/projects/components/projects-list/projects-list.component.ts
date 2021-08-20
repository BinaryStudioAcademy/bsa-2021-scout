import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { ProjectsAddComponent } from '../projects-add/projects-add.component';
import { ProjectInfo } from 'src/app/projects/models/project-info';
import { ProjectService } from 'src/app/projects/services/projects.service';
import { ProjectsEditComponent } from '../projects-edit/projects-edit.component';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ProjectDeleteConfirmComponent } 
  from '../project-delete-confirm/project-delete-confirm.component';

const TAGS: string[] = [
  'ASP.NET', 'WPF','Angular',
];


@Component({
  selector: 'app-projects-list',
  templateUrl: './projects-list.component.html',
  styleUrls: ['./projects-list.component.scss'],
})

export class ProjectsListComponent implements AfterViewInit, OnInit {
  displayedColumns: string[] =
  ['position', 'name', 'description', 'teamInfo', 'creationDate', 'tags', 'vacancies', 'actions'];
  projects: ProjectInfo[] = [];
  dataSource: MatTableDataSource<ProjectInfo>;
  tags: string[] = TAGS;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;

  constructor(
    private projectService: ProjectService,
    private dialog: MatDialog,
    private notificationService: NotificationService) {
    this.dataSource = new MatTableDataSource<ProjectInfo>();
  }

  public getProjects() {
    this.projectService
      .getProjects()
      .subscribe(
        (resp) => {
          this.projects = resp.body!;
          this.dataSource.data = this.projects;
          this.directive.applyFilter$.emit();
        },
      );
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  ngOnInit() {
    this.getProjects();
  }


  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  public OnCreate(): void {
    this.dialog.open(ProjectsAddComponent);

    this.dialog.afterAllClosed.subscribe(_ =>
      this.getProjects());
  }

  public OnEdit(projectToEdit: ProjectInfo): void {
    this.dialog.open(ProjectsEditComponent, {
      data: {
        project: projectToEdit,
      },
    });

    this.dialog.afterAllClosed.subscribe(_ =>
      this.getProjects());
  }

  public showDeleteConfirmDialog(projectToDelete: ProjectInfo): void {
    const dialogRef = this.dialog.open(ProjectDeleteConfirmComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
    });

    dialogRef.afterClosed()
      .subscribe((response: boolean) => {
        if (response) {
          this.projectService.deleteProject(projectToDelete).subscribe(_ => {
            this.notificationService.showSuccessMessage(`Project ${projectToDelete.name} deleted!`);
            this.getProjects();
          });
        }
      });
  }
}
