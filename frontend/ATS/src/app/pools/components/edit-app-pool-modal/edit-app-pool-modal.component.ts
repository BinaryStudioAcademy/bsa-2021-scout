import {COMMA, ENTER} from '@angular/cdk/keycodes';
// eslint-disable-next-line max-len
import {Component, ElementRef, ViewChild, Inject, OnInit, Output, EventEmitter} from '@angular/core';
import {Subject} from 'rxjs';
import {MatAutocompleteSelectedEvent,MatAutocomplete} from '@angular/material/autocomplete';
import {MatChipInputEvent} from '@angular/material/chips';
import {Observable} from 'rxjs';
import { map, startWith, takeUntil} from 'rxjs/operators';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Applicant } from 'src/app/shared/models/applicant/applicant';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import { ApplicantsPool } from 'src/app/shared/models/applicants-pool/applicants-pool'; 
import { ApplicantsService } from 'src/app/shared/services/applicants.service';


@Component({
  selector: 'app-edit-app-pool-modal',
  templateUrl: './edit-app-pool-modal.component.html',
  styleUrls: ['./edit-app-pool-modal.component.scss',
    '../create-talentpool-modal/create-talentpool-modal.component.scss',
  ],
})



export class EditAppPoolModalComponent implements OnInit{
  
  @Output() submitClicked = new EventEmitter<any>();
  id : string = '';
  selectable = true;
  removable = true;
  separatorKeysCodes: number[] = [ENTER, COMMA];  
  filteredApplicants: Observable<Applicant[]>;
  filterValue : string = '';

  public allapplicants : Applicant [] = [
  ];

  public applicants : Applicant [] = this.data.applicants;  

  applicantsCtrl = new FormControl();
  private unsubscribe$ = new Subject<void>();
   

  public editPool: FormGroup = new FormGroup({
    'name': new FormControl(this.data.name, [
      Validators.required,            
    ]),
    'description': new FormControl(this.data.description, [
      Validators.required,
    ]),
    applicants : this.applicantsCtrl,
  });

  
  @ViewChild('applicantInput') applicantInput!: ElementRef<HTMLInputElement>;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: ApplicantsPool,
    public dialogRef: MatDialogRef<EditAppPoolModalComponent>,
    private applicantsService: ApplicantsService,
  ) {
    this.filteredApplicants = this.applicantsCtrl.valueChanges.pipe(
      startWith(null),
      map((filterValue: string | null) => 
        filterValue ? this._filter(filterValue) 
          : this.allapplicants.slice()));
  }

  ngOnInit() {
    this.getApplicants();        
  }


  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();    
    console.log(value);
    
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

  remove(applicant: Applicant): void {
    const index = this.applicants.indexOf(applicant);

    if (index >= 0) {
      let app = this.allapplicants.find(x=>x.id === applicant.id);
      app!.isSelected = false;
      this.applicants.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    //console.log(event);
    const value = event.option.viewValue;
    console.log(value);
    
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

  private _filter(value: string): Applicant[] {    
    console.log(value);
    const filterValue = value ? value.toLowerCase() : '';    

    return this.allapplicants.filter(applicant => (
      applicant.firstName.toLowerCase().includes(filterValue) || 
      applicant.lastName.toLowerCase().includes(filterValue)
    ));
  }

  getStyle(applicant :Applicant)
  {
    return applicant.isSelected ? 'used' : '';
  }

  save() {
    const data = this.editPool.value;
    data.id = this.id;
    data.applicantsIds = this.applicants.map(x=>x.id).join();    
    this.submitClicked.emit(data);
    this.dialogRef.close();
  }

  closeDialog() {
    this.dialogRef.close();
  }


  getApplicants() {
    //this.loading = true;
    this.applicantsService
      .getApplicants()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (resp) => {
          //this.loading = false;
          this.allapplicants = resp;
          this.id = this.data.id;
          this.applicants.forEach(x=> {
            x.isSelected = true;
            const all = this.allapplicants.find(y=> y.id == x.id);
            if(all) all.isSelected = true;    
          });
          
        },
        (error) => {
          //this.loading = false; 
          //this.notificationService.showErrorMessage(error);
        },
      );
  }

  


}
