import { Project } from 'src/app/shared/models/projects/project';
import {
  AfterViewInit,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';

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
import { Tag } from 'src/app/shared/models/tags/tag';
import { DeleteConfirmComponent } 
  from 'src/app/shared/components/delete-confirm/delete-confirm.component';
import { Subject } from 'rxjs';
import { mergeMap, takeUntil } from 'rxjs/operators';
import { FollowedService } from 'src/app/shared/services/followedService';
import { EntityType } from 'src/app/shared/enums/entity-type.enum';

@Component({
  selector: 'app-projects-list',
  templateUrl: './projects-list.component.html',
  styleUrls: ['./projects-list.component.scss'],
})
export class ProjectsListComponent implements AfterViewInit, OnDestroy {
  displayedColumns: string[] = [
    'position',
    'name',
    'description',
    'teamInfo',
    'creationDate',
    'tags',
    'vacancies',
    'actions',
  ];
  projects: ProjectInfo[] = [];
  dataSource: MatTableDataSource<ProjectInfo>;
  isFollowedPage: string = 'false';
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;

  public loading: boolean = false;
  private followedSet: Set<string> = new Set();
  private readonly unsubscribe$: Subject<void> = new Subject<void>();
  private readonly followedPageToken: string = 'followedProjectPage';
  constructor(
    private projectService: ProjectService,
    private dialog: MatDialog,
    private notificationService: NotificationService,
    private followService: FollowedService,
  ) {
    this.followService.getFollowed(EntityType.Project)
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap(data => {
          data.forEach(item=>this.followedSet.add(item.entityId));
          return this.projectService.getProjects();
        }))
      .subscribe((resp) => {
        this.projects = resp.body!;
        this.projects.forEach((d, i) => {
          d.position = i + 1;
          d.isFollowed = this.followedSet.has(d.id);
        });
        if(localStorage.getItem(this.followedPageToken)=='true')
          this.dataSource.data = this.projects.filter(item=>this.followedSet.has(item.id));
        else
          this.dataSource.data = this.projects;
        this.directive.applyFilter$.emit();
      },
      );
    this.dataSource = new MatTableDataSource<ProjectInfo>();
    this.isFollowedPage = localStorage.getItem(this.followedPageToken) ? 
      localStorage.getItem(this.followedPageToken)! : 'false';
  }

  public getProjects() {
    this.projectService
      .getProjects()
      .pipe(
        takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          this.projects = resp.body!;
          this.projects.forEach((d, i) => {
            d.position = i + 1;
            d.isFollowed = this.followedSet.has(d.id);
          });
          if(localStorage.getItem(this.followedPageToken)=='true')
          {
            this.dataSource.data = this.projects.filter(item=>this.followedSet.has(item.id));
          
          }
          else
          {
            this.dataSource.data = this.projects;
          }
          this.directive.applyFilter$.emit();
        },
      );
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  public switchToFollowed(){
    this.isFollowedPage = 'true';
    this.dataSource.data = this.dataSource.data.filter(vacancy=>vacancy.isFollowed);
    this.followService.switchRefreshFollowedPageToken('true', this.followedPageToken);
    this.directive.applyFilter$.emit();
  }
  public getFirstTags(project: ProjectInfo): Tag[] {
    if (!project.tags?.tagDtos) {
      return [];
    }

    return project.tags.tagDtos.length > 3
      ? project.tags.tagDtos.slice(0, 2)
      : project.tags.tagDtos;
  }

  public toggleTags(project: ProjectInfo): void {
    project.isShowAllTags = project.isShowAllTags
      ? false
      : true;
  }
  public switchAwayToAll(){
    this.isFollowedPage = 'false';
    this.dataSource.data = this.projects;
    this.followService.switchRefreshFollowedPageToken('false', this.followedPageToken);
    this.directive.applyFilter$.emit();
  }
  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }
  public onBookmark(data: ProjectInfo, perfomToFollowCleanUp: string = 'false'){
    let projetIndex:number = this.dataSource.data.findIndex(project=>project.id === data.id)!;
    data.isFollowed = !data.isFollowed;
    if(data.isFollowed)
    {
      this.followService.createFollowed(
        {
          entityId: data.id,
          entityType: EntityType.Project,
        },
      ).subscribe();
    }else
    {
      this.followService.deleteFollowed(
        EntityType.Project, data.id,
      ).subscribe();
    }
    this.dataSource.data[projetIndex] = data;
    if(perfomToFollowCleanUp == 'true'){
      this.dataSource.data = this.dataSource.data.filter(project=>project.isFollowed);
    }
    this.directive.applyFilter$.emit();
  }
  public OnCreate(): void {
    this.dialog.open(ProjectsAddComponent);

    this.dialog.afterAllClosed.subscribe((_) => this.getProjects());
  }

  public OnEdit(projectToEdit: ProjectInfo): void {
    this.dialog.open(ProjectsEditComponent, {
      data: {
        project: projectToEdit,
      },
    });

    this.dialog.afterAllClosed.subscribe((_) => this.getProjects());
  }

  public showDeleteConfirmDialog(projectToDelete: ProjectInfo): void {
    const dialogRef = this.dialog.open(DeleteConfirmComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data:{
        entityName: 'Project',
      },
    });

    dialogRef.afterClosed()
      .subscribe((response: boolean) => {
        if (response) {
          this.projectService
            .deleteProject(projectToDelete)
            .pipe(
              takeUntil(this.unsubscribe$))
            .subscribe(_ => {
              this.notificationService
                .showSuccessMessage(`Project ${projectToDelete.name} deleted!`);
              this.getProjects();
            });
        }
      });
  }
}
