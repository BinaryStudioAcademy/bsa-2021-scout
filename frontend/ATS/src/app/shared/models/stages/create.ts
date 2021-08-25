import { ActionCreate } from '../action/create';
import { Review } from '../reviews/review';

export interface StageCreate {
  id: string;
  name: string;
  type: number;
  actions: ActionCreate[];
  // stageType Type { get; set; }
  index: number;
  IsReviewable: boolean;
  vacancyId: string;
  reviews: Review[];
}
