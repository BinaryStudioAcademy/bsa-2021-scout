import { AfterViewInit, Component,
  ViewChild, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { User } from 'src/app/users/models/user';
import { UserDataService } from 'src/app/users/services/user-data.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-users-table',
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss'],
})
export class UsersTableComponent implements AfterViewInit, OnInit {
  public displayedColumns: string[] =
  ['position', 'full-name', 'email', 'creation-date', 
    'birth-date', 'email-confirmed', 'actions'];
  private users: User[] = [];
  public dataSource: MatTableDataSource<User>;
  public loading: boolean = true;
​
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;

  constructor(
    private userDataService: UserDataService,
    private notificationService: NotificationService) {
    this.dataSource = new MatTableDataSource<User>();
  }

  public getUsers() {
    this.userDataService
      .getUsersForHrLead()
      .pipe(
        finalize(() => this.loading = false),
      )
      .subscribe(
        (resp) => {
          this.users = resp;
          this.dataSource.data = this.users;
          this.directive.applyFilter$.emit();
        },
        () => this.notificationService.showErrorMessage('Something went wrong'),
      );
  }

  public ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
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

  public sortData(sort: Sort): void {
    this.dataSource.data = (this.dataSource.data as User[]).sort((a, b) => {
      const isAsc = sort.direction === 'asc';
                  
      switch (sort.active) {
        case 'full-name':
          return this.compareRows(`${a.firstName}  ${a.lastName}`,
            `${b.firstName} ${b.lastName}`, isAsc);
        case 'email':
          return this.compareRows(a.email, b.email, isAsc);
        case 'birth-date':
          return this.compareRows(a.birthDate, b.birthDate, isAsc);
        case 'creation-date':
          return this.compareRows(a.creationDate, b.creationDate, isAsc);
        case 'email-confirmed':
          return this.compareRows(a.isEmailConfirmed, b.isEmailConfirmed, isAsc);
        default:
          return 0;
      }
    });
  }

  private compareRows(a: number|string|Date|boolean, b: number|string|Date|boolean, isAsc: boolean)
    : number {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
}