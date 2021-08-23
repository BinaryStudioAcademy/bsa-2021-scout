import { Action } from '../action/action';

export interface Stage{
  id:string,
  name: string,
  type: number,
  actions : Action[],
  // stageType Type { get; set; }
  index:number,
  IsReviewable:Boolean,
  vacancyId:string,
  rates: string //?
}