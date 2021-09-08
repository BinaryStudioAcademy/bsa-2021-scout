import { COMMA, ENTER } from '@angular/cdk/keycodes';
// eslint-disable-next-line max-len
import {Component, ElementRef, ViewChild, Inject, OnInit, Output, EventEmitter} from '@angular/core';
import {Subject} from 'rxjs';
import {MatAutocompleteSelectedEvent,MatAutocomplete} from '@angular/material/autocomplete';
import {MatChipInputEvent} from '@angular/material/chips';
import {Observable} from 'rxjs';
import { map, startWith, takeUntil} from 'rxjs/operators';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ApplicantIsSelected } from 'src/app/shared/models/applicants/applicant-select';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import { ApplicantsPool } from 'src/app/shared/models/applicants-pool/applicants-pool'; 
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { NotificationService } from 'src/app/shared/services/notification.service';


@Component({
  selector: 'app-edit-app-pool-modal',
  templateUrl: './edit-app-pool-modal.component.html',
  styleUrls: [
    './edit-app-pool-modal.component.scss',
  ],
})
export class EditAppPoolModalComponent implements OnInit {
  @Output() submitClicked = new EventEmitter<any>();
  id: string = '';
  selectable = true;
  removable = true;
  separatorKeysCodes: number[] = [ENTER, COMMA];  
  filteredApplicants: Observable<ApplicantIsSelected[]>;
  filterValue : string = '';
  loading = true;

  public allapplicants : ApplicantIsSelected [] = [];

  public applicants : ApplicantIsSelected [] = this.data.applicants;

  applicantsCtrl = new FormControl();
  private unsubscribe$ = new Subject<void>();
   

  public editPool: FormGroup = new FormGroup({
    'name': new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(20)]),
    'description': new FormControl('', [
      Validators.required,
      Validators.minLength(10),
    ]),
    applicants : this.applicantsCtrl,
  });

  @ViewChild('applicantInput') applicantInput!: ElementRef<HTMLInputElement>;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: ApplicantsPool,
    public dialogRef: MatDialogRef<EditAppPoolModalComponent>,
    private notificationService:NotificationService,
    private applicantsService: ApplicantsService,
  ) {
    this.filteredApplicants = this.applicantsCtrl.valueChanges.pipe(
      startWith(null),
      map((filterValue: string | null) =>
        filterValue ? this._filter(filterValue) : this.allapplicants.slice(),
      ),
    );
  }

  ngOnInit() {
    this.getApplicants();        
  }

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();    
    
    let app = this.applicants.find(x=> x.id === value && x.lastName === value);

    // Add our applicant
    if (app) {
      app.isSelected = true;
      this.applicants.push(app);
    }

    // Clear the input value
    event.chipInput!.clear();

    this.applicantsCtrl.setValue(null);
  }

  remove(applicant: ApplicantIsSelected): void {
    const index = this.applicants.indexOf(applicant);

    if (index >= 0) {
      let app = this.allapplicants.find((x) => x.id === applicant.id);
      app!.isSelected = false;
      this.applicants.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.viewValue;
    
    let app = this.allapplicants.find(x=> 
      `done ${x.firstName} ${x.lastName}` === value );
    if(app && !app.isSelected)
    {
      app.isSelected = true;
      this.applicants.push(app);
      this.applicantInput!.nativeElement.value = '';
      this.applicantsCtrl.setValue(null);
    }
  }

  private _filter(value: string): ApplicantIsSelected[] {    
    const filterValue = value ? value.toLowerCase() : '';    

    return this.allapplicants.filter(
      (applicant) =>
        applicant.firstName.toLowerCase().includes(filterValue) ||
        applicant.lastName.toLowerCase().includes(filterValue),
    );
  }

  getStyle(applicant :ApplicantIsSelected)
  {
    return applicant.isSelected ? 'used' : '';
  }

  save() {
    const data = this.editPool.value;
    data.id = this.id;
    data.applicantsIds = this.applicants.map((x) => x.id).join();
    this.submitClicked.emit(data);
    this.dialogRef.close();
  }

  closeDialog() {
    this.dialogRef.close();
  }

  getApplicants() {    
    this.applicantsService
      .getApplicants()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {          
          this.allapplicants = resp.map(
            value => {
              return {
                ...value, isSelected : false};                
            },            
          );
          this.id = this.data.id;
          this.editPool.controls['name'].setValue(this.data.name);
          this.editPool.controls['description'].setValue(this.data.description);
          this.applicants.forEach(x=> {
            x.isSelected = true;
            const all = this.allapplicants.find(y=> y.id == x.id);
            if(all) all.isSelected = true;    
          });
          
        },
        (error) => {                    
          this.notificationService.showErrorMessage(error.description);
          if(error.type === 'InvalidToken') this.closeDialog();

        },
        () => { this.loading = false; },
      );
  }

  getClass(control:string)
  {
    return this.editPool.controls[control].dirty && this.editPool.controls[control].errors?
      'invalid-input':'';
  }

  


}
