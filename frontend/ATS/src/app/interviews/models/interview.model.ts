import { Applicant } from 'src/app/shared/models/applicants/applicant';
import { Vacancy } from 'src/app/applicants/models/Vacancy';
import { User } from 'src/app/users/models/user';
import { MeetingSource } from './enums/meeting-source.enum';
import { InterviewType } from './enums/interview-type.enum';

export interface Interview{
  id: string,
  title: string,
  meetingLink: string,
  interviewType: InterviewType,
  meetingSource: MeetingSource,
  userParticipants: User[],
  vacancy: Vacancy,
  scheduled: string,
  duration: number,
  isReviewed: boolean,
  candidate: Applicant,
  createdDate: Date,
  note: string,
  vacancyId: string,
  candidateId: string,
}