import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VacanciesTableComponent } from './vacancies-table.component';

describe('VacanciesTableComponent', () => {
  let component: VacanciesTableComponent;
  let fixture: ComponentFixture<VacanciesTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VacanciesTableComponent ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VacanciesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
