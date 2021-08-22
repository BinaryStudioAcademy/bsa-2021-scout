import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicantDeleteConfirmComponent } from './applicant-delete-confirm.component';

describe('ApplicantDeleteConfirmComponent', () => {
  let component: ApplicantDeleteConfirmComponent;
  let fixture: ComponentFixture<ApplicantDeleteConfirmComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicantDeleteConfirmComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicantDeleteConfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
