import { ElasticEntity } from '../elastic-entity/elastic-entity';
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
  tags: ElasticEntity, //Elastic Entity???
  stages:Stage[]
} 