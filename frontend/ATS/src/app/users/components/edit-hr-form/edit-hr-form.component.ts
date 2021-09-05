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
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
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
    public imageFile: File | undefined;
    public imageUrl:string | undefined;
  
    constructor(
      public dialogRef: MatDialogRef<EditHrFormComponent>,
      // @Inject(MAT_DIALOG_DATA) public data: { userToEdit: UserTableData },
      @Inject(MAT_DIALOG_DATA) public data: { userToEdit: UserTableData, isUserLeadProfile:Boolean},
      private fb: FormBuilder,
      public userService: UserDataService,
      public notificationService: NotificationService,
    ) {
      this.profileForm = new FormGroup({
        'firstName': new FormControl({value:''}, Validators.required),
        'lastName': new FormControl({value:''}, Validators.required),    
        'birthDay': new FormControl({value:'', disabled:true}),
        'phone': new FormControl({value:''}),
        'skype': new FormControl({value:''}),
        'email': new FormControl({value:'', disabled:true}),
        // 'image': new FormControl({value:''}),
      });
      this.loading = false;
    }
  
    public ngOnDestroy(): void {
      this.unsubscribe$.next();
      this.unsubscribe$.complete();
    }
  
    ngOnInit() {
      console.log(this.data.userToEdit)
      if (!this.data.isUserLeadProfile) {
        this.userService.getByToken().subscribe(
          response => {
            this.profileForm.setValue({
              firstName: response.firstName,
              lastName: response.lastName,
              birthDay: response.birthDate,
              phone: response.phone || '',
              skype: response.skype || '',
              email: response.email,
              // image: response.avatar || ''
            });
            this.imageUrl = response.avatarUrl;
            console.log(this.imageUrl)
          });
      }else{
        let response = this.data.userToEdit;
            this.profileForm.setValue({
              firstName: response.firstName,
              lastName: response.lastName,
              birthDay: response.birthDate,
              phone: response.phone || '',
              skype: response.skype || '',
              email: response.email,
              // image: response.avatar || ''
            });
            this.imageUrl = response.avatarUrl;
            console.log(this.imageUrl)
            console.log(this.profileForm)
      }
    }
  
    //------------------VACANCY------------------
    createUser() {
      this.submitted = true;
      this.loading = true;
  
      let createUser:UserCreate = {
        id: this.data.userToEdit.id,
        firstName: this.profileForm.controls['firstName'].value,
        lastName: this.profileForm.controls['lastName'].value,
        birthDate: this.profileForm.controls['birthDay'].value,
        avatar: this.imageFile,
        skype:this.profileForm.controls['skype'].value,
        phone: this.profileForm.controls['phone'].value,
        email:this.profileForm.controls['email'].value
      };

      console.log(createUser)
      
        this.userService
          .putUser(createUser)
          .pipe(takeUntil(this.unsubscribe$))
          .subscribe(
            (response) => {
              this.loading = false;
              this.notificationService.showSuccessMessage(`The user ${createUser.firstName} ${createUser.lastName} was successfully updated.`);
            },
            () => {
              this.loading = false;
              this.notificationService.showErrorMessage('Failed to update user.');
            },
          );
  
  
      this.dialogRef.close();
    }

    public handleFileInput(target: any) {
      this.imageFile = target.files[0];

      if (!this.imageFile) {
          target.value = '';
          return;
      }

      if (this.imageFile.size / 1000000 > 5) {
          this.notificationService.showErrorMessage(`Image can't be heavier than ~5MB`);
          target.value = '';
          return;
      }

      const reader = new FileReader();
      reader.addEventListener('load', () => (this.imageUrl = reader.result as string));
      reader.readAsDataURL(this.imageFile);
  }
  
    get profileFormControl() {
      return this.profileForm.controls;
    }
  
  }
  
