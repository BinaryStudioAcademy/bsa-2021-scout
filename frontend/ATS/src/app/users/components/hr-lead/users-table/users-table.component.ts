import { FollowedService } from 'src/app/shared/services/followedService';
import { AfterViewInit, Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { UserDataService } from 'src/app/users/services/user-data.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { takeUntil, mergeMap } from 'rxjs/operators';
import { SendingRegisterLinkDialogComponent }
  from '../send-registration-link-dialog/sending-register-link-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { UserTableData } from 'src/app/users/models/user-table-data';
import { Subject } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { EntityType } from 'src/app/shared/enums/entity-type.enum';

@Component({
  selector: 'app-users-table',
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss'],
})
export class UsersTableComponent implements AfterViewInit, OnDestroy {
  public displayedColumns: string[] =
  ['position', 'full-name', 'email', 
    'birth-date', 'creation-date', 'actions'];
  private users: UserTableData[] = [];
  public dataSource: MatTableDataSource<UserTableData>;
  public loading: boolean = true;
  public isFollowedPage: boolean = false;
  private followedSet: Set<string> = new Set();
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild(MatSort) sort!: MatSort;
  private readonly followedPageToken: string = 'followedUserPage';
  constructor(
    private userDataService: UserDataService,
    private notificationService: NotificationService,
    private followService: FollowedService,
    private dialog: MatDialog) {
    this.dataSource = new MatTableDataSource<UserTableData>();
    this.followService.getFollowed(EntityType.User)
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap(data => {
          data.forEach(item => this.followedSet.add(item.entityId));
          return this.userDataService
            .getUsersForHrLead();
        },
        ),
        finalize(() => this.loading = false),
      )
      .subscribe((resp) => {
        resp.forEach((user, index) => {
          user.position = index + 1;
          user.isFollowed = this.followedSet.has(user.id ?? '');
        });
        this.users = resp;
        if (localStorage.getItem(this.followedPageToken) !== null) {
          this.dataSource.data = this.users.filter(item => item.isFollowed);
        }
        else {
          this.dataSource.data = this.users;
        }
        this.directive.applyFilter$.emit();
      },
      () => {
        this.notificationService.showErrorMessage('Something went wrong');
      });
    this.isFollowedPage = localStorage.getItem(this.followedPageToken) !== null;
  }
  public getUsers() {
    this.userDataService
      .getUsersForHrLead()
      .pipe(
        takeUntil(this.unsubscribe$),
        finalize(() => this.loading = false),
      )
      .subscribe(
        (resp) => {
          resp.forEach((user, index) => {
            user.position = index + 1;
            user.isFollowed = this.followedSet.has(user.id ?? '');
          });
          this.users = resp;
          if (localStorage.getItem(this.followedPageToken) !== null)
            this.dataSource.data = this.users.filter(item => item.isFollowed);
          else
            this.dataSource.data = this.users;
          this.directive.applyFilter$.emit();
        },
        () => {
          this.notificationService.showErrorMessage('Something went wrong');
        },
      );
  }

  public ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;

    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'full-name': return `${item.firstName}  ${item.lastName}`;
        case 'email': return item.email;
        case 'birth-date': return item.birthDate;
        case 'creation-date': return item.creationDate;
        case 'email-confirmed': return item.isEmailConfirmed;
        default: return (item as IIndexable)[property];
      }
    };
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.directive.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  public OpenSendRegistrationLinkDialog(): void {
    this.dialog.open(SendingRegisterLinkDialogComponent, {
      disableClose: true,
      maxWidth: '400px',
    });
  }

  public switchToFollowed() {
    this.isFollowedPage = true;
    this.dataSource.data = this.dataSource.data.filter(user => user.isFollowed);
    this.followService.switchRefreshFollowedPageToken(true, this.followedPageToken);
    this.directive.applyFilter$.emit();
  }

  public switchAwayToAll() {
    this.isFollowedPage = false;
    this.dataSource.data = this.users;
    this.followService.switchRefreshFollowedPageToken(false, this.followedPageToken);
    this.directive.applyFilter$.emit();
  }

  public onBookmark(data: UserTableData, perfomToFollowCleanUp: boolean = false) {
    data.isFollowed = !data.isFollowed;
    if (data.isFollowed) {
      this.followService.createFollowed(
        {
          entityId: data.id ?? '',
          entityType: EntityType.User,
        },
      ).subscribe();
    } else {
      this.followService.deleteFollowed(
        EntityType.User, data.id ?? '',
      ).subscribe();
    }
    if (perfomToFollowCleanUp) {
      this.dataSource.data = this.dataSource.data.filter(user => user.isFollowed);
    }
    this.directive.applyFilter$.emit();
  }
}

interface IIndexable {
  [key: string]: any;
}
