import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VacancyWidgetComponent } from './vacancy-widget.component';

describe('VacancyWidgetComponent', () => {
  let component: VacancyWidgetComponent;
  let fixture: ComponentFixture<VacancyWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VacancyWidgetComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VacancyWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
