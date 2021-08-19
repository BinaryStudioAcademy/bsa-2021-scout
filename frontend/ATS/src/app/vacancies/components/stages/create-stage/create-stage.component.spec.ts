import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateStageComponent } from './create-stage.component';

describe('CreateStageComponent', () => {
  let component: CreateStageComponent;
  let fixture: ComponentFixture<CreateStageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateStageComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateStageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
