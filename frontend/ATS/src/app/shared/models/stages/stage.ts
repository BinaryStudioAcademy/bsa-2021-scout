export interface Stage{
  id:string,
  name: string,
  // stageType Type { get; set; }
  index:number,
  isReviewRequired:Boolean,
  vacancyId:string,
  action:string, //?
  rates: string //?
}