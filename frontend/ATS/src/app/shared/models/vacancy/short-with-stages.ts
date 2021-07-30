import { Model } from '../model';
import { StageWithCandidates } from '../stages/with-candidates';

export interface ShortVacancyWithStages extends Model {
  title: string;
  stages: StageWithCandidates[];
}
