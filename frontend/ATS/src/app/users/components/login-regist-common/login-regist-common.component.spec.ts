import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginRegistCommonComponent } from './login-regist-common.component';

describe('LoginRegistCommonComponent', () => {
  let component: LoginRegistCommonComponent;
  let fixture: ComponentFixture<LoginRegistCommonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginRegistCommonComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginRegistCommonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
