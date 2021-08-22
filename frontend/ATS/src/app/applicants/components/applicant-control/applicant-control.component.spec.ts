import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicantControlComponent } from './applicant-control.component';

describe('ApplicantControlComponent', () => {
  let component: ApplicantControlComponent;
  let fixture: ComponentFixture<ApplicantControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicantControlComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicantControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
