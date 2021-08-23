import { ElasticEntity } from '../elastic-entity/elastic-entity';
import { StageCreateDto } from '../stages/stage-create-dto';

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
  stages:StageCreateDto[]
} 