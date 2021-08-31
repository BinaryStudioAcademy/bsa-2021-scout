import { Component, OnDestroy } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ApplicantCvService } from 'src/app/shared/services/applicant-cv.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { CvParsingStartedModalComponent }
  from '../cv-parsing-started-modal/cv-parsing-started-modal.component';

@Component({
  selector: 'app-start-cv-parsing-modal',
  templateUrl: './start-cv-parsing-modal.component.html',
  styleUrls: ['./start-cv-parsing-modal.component.scss'],
})
export class StartCvParsingModalComponent implements OnDestroy {
  public file?: File;
  public loading: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly service: ApplicantCvService,
    private readonly dialog: MatDialogRef<StartCvParsingModalComponent>,
    private readonly dialogService: MatDialog,
    private readonly notifications: NotificationService,
  ) {}

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public upload(files: File[]): void {
    this.file = files.length > 0 ? files[0] : undefined;
  }

  public startParsing(): void {
    this.service
      .startCvParsing(this.file!)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        () => {
          this.loading = false;

          this.dialogService.open(CvParsingStartedModalComponent, {
            maxWidth: '300px',
          });

          this.dialog.close();
        },
        () => {
          this.loading = false;
          this.notifications.showErrorMessage('Failed to start cv parsing');
        },
      );
  }
}
