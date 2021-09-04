import { ShortVacancy } from './short-vacancy';

export interface HomeWidgetData {
  iconName : string;
  count : number;
  description : string;
  list? : ShortVacancy[];
}