import { AfterViewInit, Component,
  ViewChild, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { UserDataService } from 'src/app/users/services/user-data.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { finalize } from 'rxjs/operators';
import { SendingRegisterLinkDialogComponent } 
  from '../send-registration-link-dialog/sending-register-link-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { UserTableData } from 'src/app/users/models/user-table-data';

@Component({
  selector: 'app-users-table',
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss'],
})
export class UsersTableComponent implements AfterViewInit, OnInit {
  public displayedColumns: string[] =
  ['position', 'full-name', 'email', 'birth-date', 
    'creation-date', 'email-confirmed', 'actions'];
  private users: UserTableData[] = [];
  public dataSource: MatTableDataSource<UserTableData>;
  public loading: boolean = true;
  public isFollowedPage: boolean = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private userDataService: UserDataService,
    private notificationService: NotificationService,
    private dialog: MatDialog) {
    this.dataSource = new MatTableDataSource<UserTableData>();
  }

  public getUsers() {
    this.userDataService
      .getUsersForHrLead()
      .pipe(
        finalize(() => this.loading = false),
      )
      .subscribe(
        (resp) => {
          resp.forEach((user, index) => {
            user.position = index + 1;
          });
          this.users = resp;
          this.dataSource.data = this.users;
          this.directive.applyFilter$.emit();
        },
        () => this.notificationService.showErrorMessage('Something went wrong'),
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
  ​
  public ngOnInit() {
    this.getUsers();
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

  public switchToFollowed(){
    this.isFollowedPage = true;
    this.dataSource.data = this.dataSource.data.filter(user => user.isFollowed);
    this.directive.applyFilter$.emit();
  }

  public switchAwayToAll(){
    this.isFollowedPage = false;
    this.dataSource.data = this.users;
    this.directive.applyFilter$.emit();
  }

  public onBookmark(data: UserTableData, perfomToFollowCleanUp: boolean = false){
    data.isFollowed = !data.isFollowed;
    if(perfomToFollowCleanUp){
      this.dataSource.data = this.dataSource.data.filter(user=> user.isFollowed);
    }
    this.directive.applyFilter$.emit();
  }
}

interface IIndexable {
  [key: string]: any;
}