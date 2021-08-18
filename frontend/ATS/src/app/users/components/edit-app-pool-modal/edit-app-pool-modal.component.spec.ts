import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAppPoolModalComponent } from './edit-app-pool-modal.component';

describe('EditAppPoolModalComponent', () => {
  let component: EditAppPoolModalComponent;
  let fixture: ComponentFixture<EditAppPoolModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditAppPoolModalComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditAppPoolModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
