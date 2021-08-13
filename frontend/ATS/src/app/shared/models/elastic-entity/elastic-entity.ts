import { Tag } from '../tags/tag';
import { ElasticType } from './elastic-type';

export interface ElasticEntity {
  elasticType: ElasticType,
  tagDtos: Tag[]
}