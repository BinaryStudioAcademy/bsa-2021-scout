import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateTalentpoolModalComponent } from './create-talentpool-modal.component';

describe('CreateTalentpoolModalComponent', () => {
  let component: CreateTalentpoolModalComponent;
  let fixture: ComponentFixture<CreateTalentpoolModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateTalentpoolModalComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateTalentpoolModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
