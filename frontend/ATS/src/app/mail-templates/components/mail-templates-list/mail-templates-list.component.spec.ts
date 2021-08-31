import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MailTemplatesListComponent } from './mail-templates-list.component';

describe('MailTemplatesListComponent', () => {
  let component: MailTemplatesListComponent;
  let fixture: ComponentFixture<MailTemplatesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MailTemplatesListComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MailTemplatesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
