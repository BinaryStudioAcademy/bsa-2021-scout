export interface Stage{
  id:string,
  name: string,
  // stageType Type { get; set; }
  index:number,
  IsReviewable:Boolean,
  vacancyId:string,
  action:string, //?
  rates: string //?
}