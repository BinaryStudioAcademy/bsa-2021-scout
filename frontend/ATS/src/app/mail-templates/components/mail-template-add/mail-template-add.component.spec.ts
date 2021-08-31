import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MailTemplateAddComponent } from './mail-template-add.component';

describe('MailTemplateAddComponent', () => {
  let component: MailTemplateAddComponent;
  let fixture: ComponentFixture<MailTemplateAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MailTemplateAddComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MailTemplateAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
