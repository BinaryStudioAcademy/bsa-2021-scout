import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationPoolComponent } from './application-pool.component';

describe('ApplicationPoolComponent', () => {
  let component: ApplicationPoolComponent;
  let fixture: ComponentFixture<ApplicationPoolComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicationPoolComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicationPoolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
