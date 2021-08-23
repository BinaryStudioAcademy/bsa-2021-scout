import { ActionCreateDto } from '../action/action-create-dto';

export interface StageCreateDto{
  id:string,
  name: string,
  type: number,
  actions : ActionCreateDto[],
  // stageType Type { get; set; }
  index:number,
  IsReviewable:Boolean,
  vacancyId:string,
  rates: string //?
}