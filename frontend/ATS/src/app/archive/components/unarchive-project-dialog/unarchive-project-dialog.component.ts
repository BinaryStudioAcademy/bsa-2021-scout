import { Component, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl } from '@angular/forms';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { ArchivedProject } from '../../models/archived-project';
import { ArchivedVacancyShort } from '../../models/archived-vacancy-short';
import { ArchivationService } from '../../services/archivation.service';
import { finalize, takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-unarchive-project-dialog',
  templateUrl: './unarchive-project-dialog.component.html',
  styleUrls: ['./unarchive-project-dialog.component.scss'],
})
export class UnarchiveProjectDialogComponent implements OnDestroy {
  public vacanciesForm = new FormControl();
  public loading: boolean = false;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();
 
  constructor(
    @Inject(MAT_DIALOG_DATA) public archivedProject: ArchivedProject,
    private readonly notificationService: NotificationService,
    private readonly archivationService: ArchivationService,
    private readonly dialogRef: MatDialogRef<UnarchiveProjectDialogComponent>) { 
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public unarhiveProject() {
    this.loading = true;
    let selectedVacancies: ArchivedVacancyShort[] = this.vacanciesForm.value;
    if (selectedVacancies) {
      this.archivedProject.vacancies = selectedVacancies;
    } else {
      this.archivedProject.vacancies = [];
    }
    
    this.archivationService.unarchiveProject(this.archivedProject)
      .pipe(
        takeUntil(this.unsubscribe$),
        finalize(() => this.loading = false),
      )
      .subscribe(
        () => this.dialogRef.close(true),
        (error) => this.notificationService
          .showErrorMessage(
            error?.message ?
              error.message : 'Project unarhivation is failed!'),
      );
  }

  public onFormClose() {
    this.dialogRef.close(false);
  }
}
