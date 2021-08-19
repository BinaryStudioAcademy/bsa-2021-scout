import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResendEmailAfterLoginComponent } from './resend-email-after-login.component';

describe('ResendEmailAfterLoginComponent', () => {
  let component: ResendEmailAfterLoginComponent;
  let fixture: ComponentFixture<ResendEmailAfterLoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ResendEmailAfterLoginComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ResendEmailAfterLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
