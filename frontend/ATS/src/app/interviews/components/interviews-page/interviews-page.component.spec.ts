import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewsPageComponent } from './interviews-page.component';

describe('InterviewsPageComponent', () => {
  let component: InterviewsPageComponent;
  let fixture: ComponentFixture<InterviewsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InterviewsPageComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InterviewsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
