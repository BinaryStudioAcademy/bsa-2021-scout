import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationBoxComponent } from './registration-box.component';

describe('RegistrationBoxComponent', () => {
  let component: RegistrationBoxComponent;
  let fixture: ComponentFixture<RegistrationBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegistrationBoxComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrationBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
