import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SendingRegisterLinkDialogComponent } from './sending-register-link-dialog.component';

describe('SendingRegisterLinkDialogComponent', () => {
  let component: SendingRegisterLinkDialogComponent;
  let fixture: ComponentFixture<SendingRegisterLinkDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SendingRegisterLinkDialogComponent ],
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SendingRegisterLinkDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
