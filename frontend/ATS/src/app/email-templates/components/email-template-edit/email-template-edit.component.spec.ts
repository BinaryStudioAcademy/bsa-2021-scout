import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailTemplateEditComponent } from './email-template-edit.component';

describe('EmailTemplateEditComponent', () => {
  let component: EmailTemplateEditComponent;
  let fixture: ComponentFixture<EmailTemplateEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmailTemplateEditComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailTemplateEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
