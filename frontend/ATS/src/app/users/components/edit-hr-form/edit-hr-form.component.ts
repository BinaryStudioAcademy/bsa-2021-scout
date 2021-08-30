import {
  Component,
  EventEmitter,
  Inject,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Project } from 'src/app/shared/models/projects/project';
import { Stage } from 'src/app/shared/models/stages/stage';
import { VacancyCreate } from 'src/app/shared/models/vacancy/vacancy-create';
import { VacancyFull } from 'src/app/shared/models/vacancy/vacancy-full';
import { ProjectService } from 'src/app/shared/services/project.service';
import { VacancyService } from 'src/app/shared/services/vacancy.service';
import { MatChipInputEvent } from '@angular/material/chips';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { VacancyData } from 'src/app/shared/models/vacancy/vacancy-data';
import { Tag } from 'src/app/shared/models/tags/tag';
import { ElasticEntity } from 'src/app/shared/models/elastic-entity/elastic-entity';
import { ElasticType } from 'src/app/shared/models/elastic-entity/elastic-type';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { User } from '../../models/user';
import { UserTableData } from '../../models/user-table-data';
import { UserDataService } from '../../services/user-data.service';
import { UserCreate } from '../../models/user-create';


@Component({
  selector: 'app-edit-hr-form',
  templateUrl: './edit-hr-form.component.html',
  styleUrls: ['./edit-hr-form.component.scss']
})
export class EditHrFormComponent implements OnInit {
    profileForm!: FormGroup;
    submitted: Boolean = false;
    // @Input()currUser!:User;
    public loading: boolean = true;
    private readonly unsubscribe$: Subject<void> = new Subject<void>();
  
    constructor(
      public dialogRef: MatDialogRef<EditHrFormComponent>,
      // @Inject(MAT_DIALOG_DATA) public data: { userToEdit: UserTableData },
      @Inject(MAT_DIALOG_DATA) public data: { userToEdit: User},
      private fb: FormBuilder,
      public userService: UserDataService,
      public notificationService: NotificationService,
    ) {
      this.profileForm = this.fb.group(
        {
          firstName: ['', [Validators.required]],
          lastName: ['', [Validators.required]],
          birthDay: ['', [Validators.required]],
          phone: [''],
          skype: [''],
          email: ['', [Validators.required]],
          image: ['', [Validators.required]]
        }
      );
      this.loading = false;
    }
  
    public ngOnDestroy(): void {
      this.unsubscribe$.next();
      this.unsubscribe$.complete();
    }
  
    ngOnInit() {
      console.log(this.data.userToEdit)
      if (this.data.userToEdit) {
        // this.userService.getById(this.data.userToEdit.id!).subscribe(
        //   response => {
          let response=this.data.userToEdit;
          console.log(response)
            this.profileForm.setValue({
              firstName: this.data.userToEdit.firstName,
              lastName: response.lastName,
              birthDay: response.birthDate,
              phone: response.phone || '',
              skype: response.skype || '',
              email: response.email,
              image: response.image || ''
            });
            console.log(this.profileForm)
          // });
      }
    }
  
    //------------------VACANCY------------------
    createUser() {
      this.submitted = true;
      this.loading = true;
  
      let createUser:UserCreate = {
        firstName: this.profileForm.controls['firstName'].value,
        lastName: this.profileForm.controls['lastName'].value,
        birthDate: this.profileForm.controls['birthDate'].value,
        image: this.profileForm.controls['image'].value,
        skype:this.profileForm.controls['skype'].value,
        phone: this.profileForm.controls['phone'].value,
        email:this.profileForm.controls['email'].value
      };
      if (!this.data.userToEdit) {
        this.userService
          .postUser(createUser)
          .pipe(takeUntil(this.unsubscribe$))
          .subscribe(
            (response) => {
              this.loading = false;
            },
            () => {
              this.loading = false;
              this.notificationService.showErrorMessage('Failed to create user.');
            },
          );
      } else {
        this.userService
          .putUser(createUser, this.data.userToEdit.id!)
          .pipe(takeUntil(this.unsubscribe$))
          .subscribe(
            (response) => {
              this.loading = false;
            },
            () => {
              this.loading = false;
              this.notificationService.showErrorMessage('Failed to update user.');
            },
          );
      }
  
  
      this.dialogRef.close();
    }
  
    get profileFormControl() {
      return this.profileForm.controls;
    }
  
  }
  
