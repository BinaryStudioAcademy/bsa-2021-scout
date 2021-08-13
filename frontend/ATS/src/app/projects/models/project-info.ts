import { VacancyInfo } from '../../shared/models/vacancy/vacancy-info';

export class ProjectInfo{
  id:string='';
  logo : string='';
  name : string='';
  description  : string='';
  teamInfo  : string='';
  websiteLink  : string='';
  companyId  : string='';
  creationDate : Date=new Date();
  vacancies : VacancyInfo[]=[];
}