import { Stage } from '../stages/stage';
import { Tag } from '../tags/tag';

export interface VacancyCreate{
  title:string
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
  tags: Tag[], //Elastic Entity???
  stages:Stage[]
} 