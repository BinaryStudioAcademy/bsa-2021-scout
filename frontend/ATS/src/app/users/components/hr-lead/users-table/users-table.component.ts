import { FollowedService } from 'src/app/shared/services/followedService';
import {
  AfterViewInit,
  Component,
  ViewChild,
  OnInit,
  OnDestroy,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { UserDataService } from 'src/app/users/services/user-data.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { takeUntil, mergeMap } from 'rxjs/operators';
// eslint-disable-next-line
import { SendingRegisterLinkDialogComponent }
  from '../send-registration-link-dialog/sending-register-link-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { UserTableData } from 'src/app/users/models/user-table-data';
import { Subject } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { EntityType } from 'src/app/shared/enums/entity-type.enum';
import {
  FilterDescription,
  FilterType,
  PageDescription,
  TableFilterComponent,
} from 'src/app/shared/components/table-filter/table-filter.component';
import { EditHrFormComponent } from '../../edit-hr-form/edit-hr-form.component';
import { PendingRegistrationsComponent }
  from '../pending-registrations/pending-registrations.component';

@Component({
  selector: 'app-users-table',
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss'],
})
export class UsersTableComponent implements AfterViewInit, OnDestroy {
  public displayedColumns: string[] = [
    'position',
    'full-name',
    'email',
    'birth-date',
    'creation-date',
    'actions',
  ];

  public filterDescription: FilterDescription = [
    {
      id: 'firstName',
      name: 'First name',
    },
    {
      id: 'lastName',
      name: 'Last name',
    },
    {
      id: 'email',
      name: 'Email',
    },
    {
      id: 'birthDate',
      name: 'Birth date',
      type: FilterType.Date,
    },
    {
      id: 'creationDate',
      name: 'Creation date',
      type: FilterType.Date,
    },
  ];

  public pageToken: string = 'followedUserPage';
  public page?: string = localStorage.getItem(this.pageToken) ?? undefined;

  public pageDescription: PageDescription = [
    {
      id: 'followed',
      selector: (user: UserTableData) => user.isFollowed ?? false,
    },
  ];

  public dataSource: MatTableDataSource<UserTableData>;
  public loading: boolean = true;
  public isFollowedPage: string = 'false';
  public users: UserTableData[] = [];

  private filteredData: UserTableData[] = [];
  private followedSet: Set<string> = new Set();
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('filter') public filter!: TableFilterComponent;

  constructor(
    private userDataService: UserDataService,
    private notificationService: NotificationService,
    private followService: FollowedService,
    private dialog: MatDialog,
  ) {
    this.dataSource = new MatTableDataSource<UserTableData>();
    this.followService
      .getFollowed(EntityType.User)
      .pipe(
        takeUntil(this.unsubscribe$),
        mergeMap((data) => {
          data.forEach((item) => this.followedSet.add(item.entityId));
          return this.userDataService.getUsersForHrLead();
        }),
        finalize(() => (this.loading = false)),
      )
      .subscribe(
        (resp) => {
          resp.forEach((user) => user.isFollowed = this.followedSet.has(user.id ?? ''));
          this.users = resp;
          this.dataSource.data = this.users;
          this.dataSource.data.forEach(x=>x.avatarUrl = x.avatarUrl ?
            x.avatarUrl+'?'+performance.now():'');
          this.directive.applyFilter$.emit();
        },
        () => {
          this.notificationService.showErrorMessage('Something went wrong');
        },
      );
      
  }

  public getUsers() {
    this.loading = true;
    this.userDataService
      .getUsersForHrLead()
      .pipe(
        takeUntil(this.unsubscribe$),
        finalize(() => (this.loading = false)),
      )
      .subscribe(
        (resp) => {
          resp.forEach((user) => user.isFollowed = this.followedSet.has(user.id ?? ''));
          this.users = resp;
          this.dataSource.data = this.users;
          this.dataSource.data.forEach(x=>x.avatarUrl = x.avatarUrl ?
            x.avatarUrl+'?'+performance.now():'');
          
          this.directive.applyFilter$.emit();
        },
        () => {
          this.notificationService.showErrorMessage('Something went wrong');
        },
      );
  }

  public setFiltered(filtered: UserTableData[]): void {
    this.filteredData = filtered;
    this.dataSource.data = this.filteredData;
    this.directive.applyFilter$.emit();
    this.dataSource.paginator?.firstPage();
  }

  public ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;

    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'full-name':
          return `${item.firstName}  ${item.lastName}`;
        case 'email':
          return item.email;
        case 'birth-date':
          return item.birthDate;
        case 'creation-date':
          return item.creationDate;
        case 'email-confirmed':
          return item.isEmailConfirmed;
        default:
          return (item as IIndexable)[property];
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

  public setPage(page?: string): void {
    this.filter.setPage(page);
    this.page = page;
  }

  public OpenSendRegistrationLinkDialog(): void {
    this.dialog.open(SendingRegisterLinkDialogComponent, {
      disableClose: true,
      maxWidth: '400px',
    });
  }

  public openPendingRegistrationsDialog(): void {
    this.dialog.open(PendingRegistrationsComponent, {
      width: '800px',
      height: '80vh',
      autoFocus: false,
    });
  }

  public onBookmark(data: UserTableData, perfomToFollowCleanUp: string = 'false') {
    data.isFollowed = !data.isFollowed;
    if (data.isFollowed) {
      this.followService
        .createFollowed({
          entityId: data.id ?? '',
          entityType: EntityType.User,
        })
        .subscribe();
    } else {
      this.followService
        .deleteFollowed(EntityType.User, data.id ?? '')
        .subscribe();
    }
    if (perfomToFollowCleanUp == 'true') {
      this.dataSource.data = this.dataSource.data.filter(user => user.isFollowed);
    }
    this.directive.applyFilter$.emit();
  }

  openEditDialog(data:UserTableData){
    const dialogRef = this.dialog.open(EditHrFormComponent, {
      width: '70%',
      height: 'auto',
      data: {userToEdit:data, isUserLeadProfile:true},
    }).afterClosed()
      .subscribe(() => this.getUsers());
  }
}

interface IIndexable {
  [key: string]: any;
}
