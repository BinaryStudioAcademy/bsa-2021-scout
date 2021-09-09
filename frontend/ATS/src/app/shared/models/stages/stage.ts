import { Action } from '../action/action';
import { Review } from '../reviews/review';

export interface Stage {
  id: string;
  name: string;
  type: number;
  actions: Action[];
  // stageType Type { get; set; }
  dataJson: string | null | undefined;
  index: number;
  IsReviewable: boolean;
  vacancyId: string;
  reviews: Review[];
}