import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { Vacancy } from 'src/app/applicants/models/Vacancy';
import { MeetingSource } from './enums/meeting-source.enum';
import { InterviewType } from './enums/interview-type.enum';
import { Interview } from './interview.model';
import { I } from '@angular/cdk/keycodes';
import { UserTableData } from 'src/app/users/models/user-table-data';

export class CreateInterviewDto{
  id?: string = '';
  title: string = '';
  meetingLink: string = '';
  interviewType: InterviewType = {} as InterviewType;
  meetingSource: MeetingSource = {} as MeetingSource;
  userParticipants: string[] = [];
  userParticipantDatas: UserTableData[] = [];
  vacancyId: string = '';
  isReviewed: boolean = true;
  scheduled: string = '';
  duration: number = 0;
  candidateId: string = '';
  note: string = '';

  constructor(interview : Interview){
    this.id = interview.id;
    this.title = interview.title;
    this.meetingLink = interview.meetingLink;
    this.interviewType = interview.interviewType;
    this.meetingSource = interview.meetingSource;
    this.vacancyId = interview.vacancyId;
    this.candidateId = interview.candidateId;
    this.scheduled = interview.scheduled;
    this.duration = interview.duration;
    this.note = interview.note;
    this.userParticipants = [];
    this.userParticipantDatas = interview.userParticipants;
    interview.userParticipants.forEach(i => {
      this.userParticipants.push(i.id as string);
    });
  }
}