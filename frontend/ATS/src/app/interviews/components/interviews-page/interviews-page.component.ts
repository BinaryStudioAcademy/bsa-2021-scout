import { InterviewsService } from './../../services/interviews.service';
import { Component, OnInit, Pipe, PipeTransform } from '@angular/core';
import { CalendarDay } from '../../models/calendar-date.model';
import { Interview } from '../../models/interview.model';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DataErrorOccurredEvent } from 'devextreme/ui/tree_list';
import { CreateInterviewComponent } from '../create-interview/create-interview.component';
import { MatDialog } from '@angular/material/dialog';
import { CreateInterviewDto } from '../../models/create-interview-dto.model';
import { S } from '@angular/cdk/keycodes';

@Pipe({
  name: 'chunk',
})
export class ChunkPipe implements PipeTransform {

  transform(calendarDaysArray: any, chunkSize: number): any {
    let calendarDays:any = [];
    let weekDays:any = [];

    calendarDaysArray.map((day:any,index:number) => {
      weekDays.push(day);
      if (++index % chunkSize  === 0) {
        calendarDays.push(weekDays);
        weekDays = [];
      }
    });
    return calendarDays;
  }
}

@Component({
  selector: 'app-interviews-page',
  templateUrl: './interviews-page.component.html',
  styleUrls: ['./interviews-page.component.scss'],
})

export class InterviewsPageComponent{
  public calendar: CalendarDay[] = [];
  public monthNames = ['January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December',
  ];
  public displayMonth: string = '';
  public currentDate: Date = new Date(Date.now());
  private readonly unsubscribe$: Subject<void> = new Subject<void>();
  private interviewsDateSet: Date[] = [];
  private interviews: Interview[] = [];
  public dateArrived!: Promise<boolean>;
  public toogleNeedReview: boolean = true;
  constructor(public service: InterviewsService,
    private dialog: MatDialog) {
    service.getInterviews()
      .pipe(
        takeUntil(this.unsubscribe$),
      )
      .subscribe((resp) => 
      {
        this.interviews = resp.body!;
        this.interviews = this.interviews.sort((a, b) =>new Date(a.scheduled).getTime() 
          - new Date(b.scheduled).getTime());
        this.interviews = this.interviews.sort((a, b) => a.isReviewed > b.isReviewed? -1 : 1);
        this.dateArrived = Promise.resolve(true);
        this.generateCalendarDays();
      },
      );
  }
  
  private date: Date = new Date();
  private generateCalendarDays(): void {
    // we reset our calendar
    this.calendar = [];
    // we set the date 
    let day: Date = this.date;
    // set the dispaly month for UI
    this.displayMonth = this.monthNames[day.getMonth()];

    let startingDateOfCalendar = this.getStartDateForCalendar(day);

    let dateToAdd = startingDateOfCalendar;

    for (var i = 0; i < 42; i++) {
      const calendarDay = new CalendarDay(new Date(dateToAdd));
     
      // eslint-disable-next-line max-len
      calendarDay.interviews = this.interviews.filter(i=>
        new Date(i.scheduled).getMonth() == dateToAdd.getMonth() && 
        new Date(i.scheduled).getDate() == dateToAdd.getDate() &&
        new Date(i.scheduled).getFullYear() == dateToAdd.getFullYear(),
      );
      this.calendar.push(calendarDay);
      dateToAdd = new Date(dateToAdd.setDate(dateToAdd.getDate() + 1));
    }
  }

  private getStartDateForCalendar(selectedDate: Date){
    // for the day we selected let's get the previous month last day
    let lastDayOfPreviousMonth = new Date(selectedDate.setDate(0));

    // start by setting the starting date of the calendar same as the last day of previous month
    let startingDateOfCalendar: Date = lastDayOfPreviousMonth;

    // but since we actually want to find the last Monday of previous month
    // we will start going back in days intil we encounter our last Monday of previous month
    if (startingDateOfCalendar.getDay() != 0) {
      do {
        startingDateOfCalendar = new Date(startingDateOfCalendar
          .setDate(startingDateOfCalendar.getDate() - 1));
      } while (startingDateOfCalendar.getDay() != 0);
    }

    return startingDateOfCalendar;
  }

  public increaseMonth() {
    const [year, month] = [this.date.getFullYear(), this.date.getMonth()];
    this.date = new Date(year, month + 2, 1);
    this.generateCalendarDays();
  }

  public decreaseMonth() {
    const [year, month] = [this.date.getFullYear(), this.date.getMonth()];
    this.date = new Date(year, month, 1);
    this.generateCalendarDays();
  }
  public collapse(calendar: CalendarDay){
    calendar.isCollapsed = !calendar.isCollapsed;
  }
  public isWeekend(date:Date):boolean{
    return (date.getDay()%6==0);
  }
  isSameMonth(date: Date) {
    return date.getMonth() === this.date.getMonth() + 1;
  }
  debug(date:Date, sched: Date){
    console.log(date);
    console.log(sched);
    console.log(date.getMonth() == sched.getMonth()
    && date.getFullYear() == sched.getFullYear()&&
    date.getDate() == sched.getDate(),
    );
  }

  public OnEdit(interview : Interview){
    var interviewToEdit: CreateInterviewDto = new CreateInterviewDto(interview);
    const dialogRef = this.dialog.open(CreateInterviewComponent, {
      data: {
        interview: interviewToEdit,
      },
    });
    dialogRef.afterClosed().subscribe(() => {
      this.service.getInterviews()
        .pipe(
          takeUntil(this.unsubscribe$),
        )
        .subscribe((resp) => 
        {
          this.interviews = resp.body!;
          this.interviewsDateSet = [];
          this.date = new Date(Date.now());
          this.interviews.forEach(
            i => {
              this.interviewsDateSet.push(new Date(i.scheduled));
            });
          this.dateArrived = Promise.resolve(true);
          this.generateCalendarDays();
        },
        );
    });
  }

  public OnCreate(): void {
    const dialogRef = this.dialog.open(CreateInterviewComponent);
    dialogRef.afterClosed().subscribe(() => {
      this.service.getInterviews()
        .pipe(
          takeUntil(this.unsubscribe$),
        )
        .subscribe((resp) => 
        {
          this.interviews = resp.body!;
          this.interviewsDateSet = [];
          this.date = new Date(Date.now());
          this.interviews.forEach(
            i => {
              this.interviewsDateSet.push(new Date(i.scheduled));
            });
          this.dateArrived = Promise.resolve(true);
          this.generateCalendarDays();
        },
        );
    });
  }

  cachedNeedReviewInterviews: Interview[] = [];
  onToogleNeedReview() {
    this.toogleNeedReview = !this.toogleNeedReview;

    this.calendar.forEach(day=>{
      if(day.isToday)
      {
        if(this.toogleNeedReview)
        {
          this.cachedNeedReviewInterviews.forEach(cachedInterview => 
            day.interviews.push(cachedInterview));
          day.interviews.sort((a, b) => a.isReviewed > b.isReviewed ? -1 : 1);
        }
        else
        {
          this.cachedNeedReviewInterviews = day.interviews
            .filter(interview => !interview.isReviewed);
          day.interviews = day.interviews.filter(interview => interview.isReviewed);
          if(day.interviews.length < 2) {
            day.isCollapsed = true;
          }
        }
      }
    });
  }
}
