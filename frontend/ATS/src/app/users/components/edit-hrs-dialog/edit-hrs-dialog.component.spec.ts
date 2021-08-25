import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditHrsDialogComponent } from './edit-hrs-dialog.component';

describe('EditHrsDialogComponent', () => {
  let component: EditHrsDialogComponent;
  let fixture: ComponentFixture<EditHrsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditHrsDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditHrsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
