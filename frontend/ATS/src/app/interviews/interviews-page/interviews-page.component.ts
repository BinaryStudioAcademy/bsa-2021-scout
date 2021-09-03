import { Component, OnInit, Pipe, PipeTransform } from '@angular/core';

export class CalendarDay {
  public date: Date;
  public title: string = '';
  public isPastDate: boolean;
  public isToday: boolean;

  public getDateString() {
    return this.date.toISOString().split('T')[0];
  }

  constructor(d: Date) {
    this.date = d;
    this.isPastDate = d.setHours(0, 0, 0, 0) < new Date().setHours(0, 0, 0, 0);
    this.isToday = d.setHours(0, 0, 0, 0) == new Date().setHours(0, 0, 0, 0);
  }

}

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

export class InterviewsPageComponent implements OnInit{
  public calendar: CalendarDay[] = [];
  public monthNames = ['January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December',
  ];
  public displayMonth: string = '';
  private monthIndex: number = 0;

  ngOnInit(): void {
    this.generateCalendarDays();
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
      this.calendar.push(new CalendarDay(new Date(dateToAdd)));
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

  public isWeekend(date:Date):boolean{
    return (date.getDay()%6==0);
  }
  isSameMonth(date: Date) {
    return date.getMonth() === this.date.getMonth() + 1;
  }
  debug(date:Date){
    console.log(date.getMonth());
    console.log(this.date);
  }
}
