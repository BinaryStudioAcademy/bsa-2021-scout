import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import moment from 'moment';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
// eslint-disable-next-line
import { AvatarModalComponent } from 'src/app/shared/components/avatar-modal/avatar-modal.component';
import { getApplicantAvatar } from 'src/app/shared/helpers/avatar';
import { FullVacancyCandidate } from 'src/app/shared/models/vacancy-candidates/full';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { VacancyCandidateService } from 'src/app/shared/services/vacancy-candidate.service';

@Component({
  selector: 'app-one-candidate',
  templateUrl: './one-candidate.component.html',
  styleUrls: ['./one-candidate.component.scss'],
})
export class OneCandidateComponent implements OnInit, OnDestroy {
  @Input() public id!: string;
  @Input() public vacancyId!: string;

  public data!: FullVacancyCandidate;
  public loading: boolean = true;

  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  public constructor(
    private readonly service: VacancyCandidateService,
    private readonly notificationService: NotificationService,
    private readonly dialog: MatDialog,
  ) {}

  public ngOnInit(): void {
    this.loadData(this.id);
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public enlargeAvatar(): void {
    this.dialog.open(AvatarModalComponent, {
      data: { url: this.getAvatar() },
    });
  }

  public getAvatar(): string {
    return getApplicantAvatar(this.data);
  }

  private loadData(id: string): void {
    this.service
      .getFull(id, this.vacancyId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (data) => {
          this.loading = false;
          this.data = data;
        },
        () => {
          this.loading = false;
          this.notificationService.showErrorMessage('Failed to load', 'Error');
        },
      );
  }
}
