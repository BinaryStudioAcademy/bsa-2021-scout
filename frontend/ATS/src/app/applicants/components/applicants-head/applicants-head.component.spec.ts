import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicantsHeadComponent } from './applicants-head.component';

describe('ApplicantsHeadComponent', () => {
  let component: ApplicantsHeadComponent;
  let fixture: ComponentFixture<ApplicantsHeadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicantsHeadComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicantsHeadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
