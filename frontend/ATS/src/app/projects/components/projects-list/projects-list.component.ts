import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
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

import { EMPTY, Subject, Subscription } from 'rxjs';
import { finalize, mergeMap, takeUntil } from 'rxjs/operators';
import { FollowedService } from 'src/app/shared/services/followedService';
import { EntityType } from 'src/app/shared/enums/entity-type.enum';
import {
  FilterDescription,
  FilterType,
  PageDescription,
  TableFilterComponent,
} from 'src/app/shared/components/table-filter/table-filter.component';
import { IOption } from 'src/app/shared/components/multiselect/multiselect.component';

import { ArchivationService } from 'src/app/archive/services/archivation.service';
import { ConfirmationDialogComponent } 
  from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { ActivatedRoute } from '@angular/router';


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
  filteredData: ProjectInfo[] = [];
  dataSource: MatTableDataSource<ProjectInfo>;
  pageToken: string = 'followedProjectPage';
  page?: string = localStorage.getItem(this.pageToken) ?? undefined;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild('filter') public filter!: TableFilterComponent;

  public loading: boolean = true;
  public filterDescription: FilterDescription = [];

  public pageDescription: PageDescription = [
    {
      id: 'followed',
      selector: (pool: ProjectInfo) => pool.isFollowed,
    },
  ];

  private followedSet: Set<string> = new Set();
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private projectService: ProjectService,
    private dialog: MatDialog,
    private notificationService: NotificationService,
    private followService: FollowedService,
    private archivationService: ArchivationService,
    private readonly route: ActivatedRoute,
  ) {
    this.followService
      .getFollowed(EntityType.Project)
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap((data) => {
          data.forEach((item) => this.followedSet.add(item.entityId));
          return this.projectService.getProjects();
        }),
        finalize(() => (this.loading = false)),
      )
      .subscribe((resp) => {
        this.projects = resp.body!;
        this.projects.forEach((d) => d.isFollowed = this.followedSet.has(d.id));

        this.dataSource.data = this.projects;
        let subscription: Subscription | null = null;

        subscription = this.route.queryParams.subscribe(params => {
          if (params.editId) {
            this.OnEdit(params.editId);
          }

          subscription?.unsubscribe();
        });

        this.renewFilterDescription();
        this.directive.applyFilter$.emit();
      });
    this.dataSource = new MatTableDataSource<ProjectInfo>();
  }

  public getProjects() {
    this.loading = true;
    this.projectService
      .getProjects()
      .pipe(
        takeUntil(this.unsubscribe$),
        finalize(() => (this.loading = false)),
      )
      .subscribe((resp) => {
        this.projects = resp.body!;
        this.projects.forEach((d) => d.isFollowed = this.followedSet.has(d.id));
        this.dataSource.data = this.projects;
        this.renewFilterDescription();
        this.directive.applyFilter$.emit();
      });
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  public setPage(page?: string): void {
    this.filter.setPage(page);
    this.page = page;
  }

  public onTagClick(tag: Tag): void {
    this.filter.extraAdd('tags', {
      id: tag.id,
      value: tag.id,
      label: tag.tagName,
    });
  }

  public renewFilterDescription(): void {
    const detectedTagIds: string[] = [];
    const tags: IOption[] = [];

    this.projects.forEach((p) =>
      p.tags.tagDtos.forEach((tag) => {
        if (!detectedTagIds.includes(tag.id)) {
          tags.push({
            id: tag.id,
            value: tag.id,
            label: tag.tagName,
          });

          detectedTagIds.push(tag.id);
        }
      }),
    );

    this.filterDescription = [
      {
        id: 'name',
        name: 'Name',
      },
      {
        id: 'description',
        name: 'Description',
      },
      {
        id: 'teamInfo',
        name: 'Team info',
      },
      {
        id: 'creationDate',
        name: 'Creation date',
        type: FilterType.Date,
      },
      {
        id: 'tags',
        name: 'Tags',
        type: FilterType.Multiple,
        multipleSettings: {
          options: tags,
          canBeExtraModified: true,
          valueSelector: (project) =>
            project.tags.tagDtos.map((tag: Tag) => tag.id),
        },
      },
      {
        id: 'vacancies',
        property: 'vacancies.length',
        name: 'Vacancies amount',
        type: FilterType.Number,
        numberSettings: {
          integer: true,
          min: 0,
        },
      },
    ];
  }

  public setFiltered(data: ProjectInfo[]): void {
    this.filteredData = data;
    this.dataSource.data = this.filteredData;
    this.directive?.applyFilter$.emit();
    this.dataSource.paginator?.firstPage();
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
    project.isShowAllTags = project.isShowAllTags ? false : true;
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
    let projetIndex: number = this.dataSource.data.findIndex(
      (project) => project.id === data.id,
    )!;
    data.isFollowed = !data.isFollowed;
    if (data.isFollowed) {
      this.followService
        .createFollowed({
          entityId: data.id,
          entityType: EntityType.Project,
        })
        .subscribe();
    } else {
      this.followService
        .deleteFollowed(EntityType.Project, data.id)
        .subscribe();
    }
    this.dataSource.data[projetIndex] = data;
    if (perfomToFollowCleanUp == 'true') {
      this.dataSource.data = this.dataSource.data.filter(
        (project) => project.isFollowed,
      );
    }
    this.directive.applyFilter$.emit();
  }

  public OnCreate(): void {
    this.dialog.open(ProjectsAddComponent, {width: '600px'});

    this.dialog.afterAllClosed.subscribe((_) => this.getProjects());
  }

  public OnEdit(projectToEdit: string): void;
  public OnEdit(projectToEdit: ProjectInfo): void;
  public OnEdit(projectToEdit: ProjectInfo | string): void {
    if (typeof projectToEdit === 'string') {
      projectToEdit = this.projects.find(p => p.id === projectToEdit)!;
    }

    this.dialog.open(ProjectsEditComponent, {
      width: '600px',
      data: {
        project: projectToEdit,
      },
    });

    this.dialog.afterAllClosed.subscribe((_) => this.getProjects());
  }

  public showArchiveConfirmDialog(projectToArchive: ProjectInfo): void {
    this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      height: 'min-content',
      autoFocus: false,
      data: {
        action: 'Archive',
        entityName: 'project',
        additionalMessage: 'All vacancies in this project will also be automatically archived.',
      },
    })
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap(response => {
          if (response) {
            return this.archivationService.archiveProject(projectToArchive.id);
          }
          return EMPTY;
        }),
      )
      .subscribe(
        (_) => {
          let position = this.projects.findIndex(project => project.id === projectToArchive.id);
          this.projects.splice(position, 1);
          
          position = this.filteredData.findIndex(vacancy => vacancy.id === projectToArchive.id);
          this.filteredData.splice(position, 1);

          this.dataSource.data = this.filteredData;
          this.renewFilterDescription();
          this.directive.applyFilter$.emit();

          this.notificationService.showSuccessMessage(
            `Project ${projectToArchive.name} arhived!`,
          );
        },
        () => {
          this.notificationService.showErrorMessage(
            'Project arhivation is failed!',
          );
        },
      );
  }
}
