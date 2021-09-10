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
import { AbstractControl, FormBuilder, FormControl, FormGroup,
  ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
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
import { LoginRegistCommonComponent } from '../login-regist-common/login-regist-common.component';


@Component({
  selector: 'app-edit-hr-form',
  templateUrl: './edit-hr-form.component.html',
  styleUrls: ['./edit-hr-form.component.scss'],
})
export class EditHrFormComponent implements OnInit, OnDestroy {
  profileForm!: FormGroup;
  submitted: Boolean = false;
  public loading: boolean = true;
  private readonly unsubscribe$: Subject<void> = new Subject<void>();
  public imageFile: File | undefined;
  public imageUrl:string | undefined;
  public isAvatarToDelete: boolean = false;
  
  constructor(
    public loginRegistCommonComponent: LoginRegistCommonComponent,
    public dialogRef: MatDialogRef<EditHrFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { userToEdit: UserTableData, isUserLeadProfile:Boolean},
    private fb: FormBuilder,
    public userService: UserDataService,
    public notificationService: NotificationService,
  ) {
    this.profileForm = new FormGroup({
      firstName: new FormControl('', [
        Validators.required,
        firstAndLastNameUpperValidation,
        this.loginRegistCommonComponent.firstAndLastNameValidation,
      ]),
      lastName: new FormControl('', [
        Validators.required,
        firstAndLastNameUpperValidation,
        this.loginRegistCommonComponent.firstAndLastNameValidation,
      ]),    
      birthDay: new FormControl({value:'', disabled:true}),
      phone: new FormControl('',
        Validators.pattern(
          '(([+][(]?[0-9]{1,3}[)]?)|([(]?[0-9]{4}[)]?))\s*[)]?'+
        '[-\s\.]?[(]?[0-9]{1,3}[)]?([-\s\.]?[0-9]{3})([-\s\.]?[0-9]{3,4})')),
      skype: new FormControl(''),
      slack: new FormControl(''),
      email: new FormControl({value:'', disabled:true}),
    });
    this.loading = false;
  }

    
  
  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  
  ngOnInit() {
    if (!this.data.isUserLeadProfile) {
      this.userService.getByToken().subscribe(
        response => {
          this.profileForm.setValue({
            firstName: response.firstName,
            lastName: response.lastName,
            birthDay: response.birthDate,
            phone: response.phone || '',
            skype: response.skype || '',
            slack: response.slack || '',
            email: response.email,
          });
          this.imageUrl = response.avatarUrl ? response.avatarUrl +'?'+performance.now() : '';

        });
    }else{
      this.userService.getById(this.data.userToEdit.id!).subscribe(
        response => {
          this.profileForm.setValue({
            firstName: response.firstName,
            lastName: response.lastName,
            birthDay: response.birthDate,
            phone: response.phone || '',
            skype: response.skype || '',
            slack: response.slack || '',
            email: response.email,
          });
          this.imageUrl = response.avatarUrl ? response.avatarUrl +'?'+performance.now() : '';

        });
    }



  }

  setAvatarToDelete(){
    let areYouSure = confirm('Are you sure you want to delete this photo?');
    if(areYouSure == true){
      this.isAvatarToDelete = true;
      this.imageUrl = '';
    }
  }
  
   
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
      slack:this.profileForm.controls['slack'].value,
      phone: this.profileForm.controls['phone'].value,
      email:this.profileForm.controls['email'].value,
      isImageToDelete: this.isAvatarToDelete,
    };
      
    this.userService
      .putUser(createUser)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (response) => {
          this.loading = false;
          this.notificationService.showSuccessMessage(`The user ${createUser.firstName} 
          ${createUser.lastName} was successfully updated.`);
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
      this.notificationService.showErrorMessage('Image can\'t be heavier than ~5MB');
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
  getFirstNameErrorMessage() {
    return this.profileFormControl.firstName.errors?.required ?
      'You must enter a value' :
      this.profileFormControl.firstName.errors?.firstandlastnameupper ?
        'Should start with upper latin letter' :
        this.profileFormControl.firstName.errors?.firstandlastname
          ? 'Only latin letters, spaces, hyphens' :
          '';
  }
  getLastNameErrorMessage() {
    return this.profileFormControl.lastName.errors?.required ? 
      'You must enter a value' :
      this.profileFormControl.lastName.errors?.firstandlastnameupper ? 
        'Should start with upper latin letter' :
        this.profileFormControl.lastName.errors?.firstandlastname ? 
          'Only latin letters, spaces, hyphens' :
          '';
  }

  
}

function firstAndLastNameUpperValidation(control: FormControl) {
  return ((control.value as string) || '').match(
    /^[A-Z]/,
  ) != null
    ? null
    : { firstandlastnameupper: true };
}

  
