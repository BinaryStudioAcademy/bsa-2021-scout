import { VacancyInfo } from './vacancy-info';
export interface VacancyWidgetInfo {
  iconName : string;
  count : number;
  description : string;
  list : VacancyInfo []
}