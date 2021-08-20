import { Stage } from '../stages/stage';
import { VacancyStatus } from './vacancy-status';

export interface VacancyFull{
  title:string,
  description:string,
  requirements:string,
  projectId:string,
  salaryFrom:number,
  salaryTo:number,
  tierFrom:number,
  tierTo:number,
  sources:string,
  isHot:boolean,
  isRemote:boolean,
  stages:Stage[],
  responsibleHrId: string,
  companyId: string,
  status: VacancyStatus,
  creationDate: Date,
  dateOfOpening:Date,
  modificationDate:Date,
  completionDate:Date,
  plannedCompletionDate:Date
} 