import { ElasticEntity } from 'src/app/shared/models/elastic-entity/elastic-entity';
import { Tag } from 'src/app/shared/models/tags/tag';
import { VacancyInfo } from '../../shared/models/vacancy/vacancy-info';

export class ProjectInfo{
  id:string='';
  isFollowed: boolean = false;
  logo : string='';
  name : string='';
  description  : string='';
  teamInfo  : string='';
  websiteLink  : string='';
  companyId  : string='';
  creationDate : Date=new Date();
  tags: ElasticEntity = new ElasticEntity();
  isShowAllTags: boolean = false;
  vacancies : VacancyInfo[]=[];
}