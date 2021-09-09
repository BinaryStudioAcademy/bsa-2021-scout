import { InterviewType } from './../../models/enums/interview-type.enum';
import { Interview } from './../../models/interview.model';
import { Component, Input, OnInit, Pipe, PipeTransform } from '@angular/core';


@Component({
  selector: 'app-interview-card',
  templateUrl: './interview-card.component.html',
  styleUrls: ['./interview-card.component.scss'],
})
export class InterviewCardComponent implements OnInit {

  @Input() interview!: Interview;
  constructor() { }

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnInit(): void {

  }
  getDateAfter(date: string, duration:number): Date{

    return new Date(new Date(date).getTime() + duration*60000);
  }
  public getDate(date:string): Date{
    return new Date(date);
  }
  public getStatus(index: number): string {
    return InterviewType[index];
  }

}
