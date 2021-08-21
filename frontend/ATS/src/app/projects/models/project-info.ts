import { ElasticEntity } from 'src/app/shared/models/elastic-entity/elastic-entity';
import { Tag } from 'src/app/shared/models/tags/tag';
import { VacancyInfo } from '../../shared/models/vacancy/vacancy-info';

export class ProjectInfo{
  id:string='';
  position: number = 0;
  isFollowed: boolean = false;
  logo : string='';
  name : string='';
  description  : string='';
  teamInfo  : string='';
  websiteLink  : string='';
  companyId  : string='';
  creationDate : Date=new Date();
  tags: ElasticEntity | undefined;
  isShowAllTags: boolean = false;
  vacancies : VacancyInfo[]=[];
}