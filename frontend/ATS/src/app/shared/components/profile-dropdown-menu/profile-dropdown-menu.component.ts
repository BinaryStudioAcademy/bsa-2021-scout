// import { Component, OnInit, Input } from '@angular/core';
// import { MatDialog } from '@angular/material/dialog';
// import { Subject } from 'rxjs';
// import { takeUntil } from 'rxjs/operators';
// import { EditHrFormComponent } from 'src/app/users/components/edit-hr-form/edit-hr-form.component';
// import { User } from '../../../users/models/user';
// import { AuthenticationService } from '../../../users/services/auth.service';

// @Component({
//   selector: 'app-profile-dropdown-menu',
//   templateUrl: './profile-dropdown-menu.component.html',
//   styleUrls: ['./profile-dropdown-menu.component.scss']
// })
// export class ProfileDropdownMenuComponent implements OnInit {
  
//   public loading: boolean = false;
//   @Input() user!:User;

//   private readonly unsubscribe$: Subject<void> = new Subject<void>();

//   public constructor(
//     private readonly service: AuthenticationService,
//     private dialog: MatDialog
//   ) {}

//   public logout(): void {
//     this.loading = true;

//     this.service
//       .logout()
//       .pipe(takeUntil(this.unsubscribe$))
//       .subscribe(
//         () => {
//           this.loading = false;
//           window.location.replace('/login');
//         },
//         () => {
//           this.loading = false;
//           window.location.replace('/login');
//         },
//       );
//   }
//   ngOnInit(): void {
//   }


//   onOpenProfile(){
//     const dialogRef = this.dialog.open(EditHrFormComponent, {
//       width: '70%',
//       height: 'auto',
//       data: {userToEdit:this.user},
//     }).afterClosed().subscribe(() => this.getUsers());;
//   }

// }
