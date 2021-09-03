import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MailTemplateEditComponent } from './mail-template-edit.component';

describe('MailTemplateEditComponent', () => {
  let component: MailTemplateEditComponent;
  let fixture: ComponentFixture<MailTemplateEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MailTemplateEditComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MailTemplateEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
