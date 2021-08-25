import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailTemplateAddComponent } from './email-template-add.component';

describe('EmailTemplateAddComponent', () => {
  let component: EmailTemplateAddComponent;
  let fixture: ComponentFixture<EmailTemplateAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmailTemplateAddComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailTemplateAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
