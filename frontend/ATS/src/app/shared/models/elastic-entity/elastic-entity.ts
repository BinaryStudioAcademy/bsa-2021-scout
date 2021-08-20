import { Tag } from '../tags/tag';
import { ElasticType } from './elastic-type';

export interface ElasticEntity {
  id: string,
  elasticType: ElasticType,
  tagDtos: Tag[]
}