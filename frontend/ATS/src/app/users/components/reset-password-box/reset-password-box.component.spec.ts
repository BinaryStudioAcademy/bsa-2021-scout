import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetPasswordBoxComponent } from './reset-password-box.component';

describe('ResetPasswordBoxComponent', () => {
  let component: ResetPasswordBoxComponent;
  let fixture: ComponentFixture<ResetPasswordBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ResetPasswordBoxComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ResetPasswordBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
