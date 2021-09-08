import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditHrFormComponent } from './edit-hr-form.component';

describe('EditHrFormComponent', () => {
  let component: EditHrFormComponent;
  let fixture: ComponentFixture<EditHrFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditHrFormComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditHrFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
