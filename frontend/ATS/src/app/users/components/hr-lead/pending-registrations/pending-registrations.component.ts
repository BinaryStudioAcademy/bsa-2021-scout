import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { StylePaginatorDirective } from 'src/app/shared/directives/style-paginator.directive';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { RegistrationPermissionShort } from 'src/app/users/models/registration-permission-short';
import { RegistrationPermissionsService }
  from 'src/app/users/services/registration-permissions.service';

@Component({
  selector: 'app-pending-registrations',
  templateUrl: 'pending-registrations.component.html',
  styleUrls: [ 'pending-registrations.component.scss' ],
})

export class PendingRegistrationsComponent implements OnDestroy, AfterViewInit {

  public cashedData: RegistrationPermissionShort[] = [];
  public dataSource: MatTableDataSource<RegistrationPermissionShort>
  = new MatTableDataSource<RegistrationPermissionShort>();

  public searchValue: string = '';
    
  public displayedColumns = [
    'position',
    'email',
    'expires_in',
    'status',
    'actions',
  ];

  private readonly unsubscribe$: Subject<void> = new Subject<void>(); 

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(StylePaginatorDirective) directive!: StylePaginatorDirective;

  constructor(
    private readonly registrationPermissionsService: RegistrationPermissionsService,
    private readonly notificationService: NotificationService,
  ) {
    this.registrationPermissionsService.getPendingRegistrations()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((data: RegistrationPermissionShort[]) => {
        this.cashedData = data;
        this.dataSource.data = data;
        this.directive.applyFilter$.emit();
      },
      (error: Error) => (this.notificationService.showErrorMessage(error.message,
        'Failed to load data')));
  }

  public ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator!;
  }

  public getLinkStatus(isActive: boolean): string {
    return isActive
      ? 'Active'
      : 'Inactive';
  }

  public applySearchValue() {
    this.searchValue = this.searchValue;
    this.dataSource.filter = this.searchValue;

    if (this.dataSource.paginator) {
      this.directive?.applyFilter$.emit();
      this.dataSource.paginator.firstPage();
    }
  }

  public resendInvitationLink(registrationPermission: RegistrationPermissionShort): void {
    this.registrationPermissionsService.resendRegistrationLink(registrationPermission)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((newRegistrationPermission: RegistrationPermissionShort) => {
        const permissionIndex = this.cashedData
          .findIndex(p => p.id === registrationPermission.id);

        this.cashedData[permissionIndex] = newRegistrationPermission;
        this.dataSource.data = this.cashedData;

        this.notificationService.showSuccessMessage('The link was successfully resent',
          'Success');

        this.directive.applyFilter$.emit();
      },
      (error: Error) => (this.notificationService.showErrorMessage(error.message,
        'An error occured while resending the link')),
      );
  }

  public revokeInvitationLink(permissionId: string): void {
    this.registrationPermissionsService.revokeRegistrationLink(permissionId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(() => {
        const permissionIndex = this.cashedData.findIndex(p => p.id === permissionId);
        this.cashedData.splice(permissionIndex, 1);
        this.dataSource.data = this.cashedData;

        this.notificationService.showSuccessMessage('The link was successfully revoked',
          'Success');

        this.directive.applyFilter$.emit();
      },
      (error: Error) => (this.notificationService.showErrorMessage(error.message,
        'An error occured while revoking the link')),
      );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public sortData(sort: Sort): void {
    this.dataSource.data = this.dataSource.data.sort(
      (a, b) => {
        const isAsc = sort.direction === 'asc';

        switch (sort.active) {
          case 'email':
            return this.compareRows(a.email, b.email, isAsc);
          case 'expires_in':
            return this.compareDates(a.date, b.date, isAsc);
          case 'status':
            return this.compareBooleans(a.isActive, b.isActive, isAsc);
          default:
            return 0;
        }
      },
    );
  }

  private compareBooleans(a: boolean, b: boolean, isAsc: boolean): number {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }

  private compareRows(
    a: number | string,
    b: number | string,
    isAsc: boolean,
  ): number {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }

  private compareDates(a: Date, b: Date, isAsc: boolean): number {
    return (a.getTime() < b.getTime() ? -1 : 1) * (isAsc ? 1 : -1);
  }
}