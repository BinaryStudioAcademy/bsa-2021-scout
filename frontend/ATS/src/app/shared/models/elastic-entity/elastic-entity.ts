import { Tag } from '../tags/tag';
import { ElasticType } from './elastic-type';

export class ElasticEntity {
  id: string='';
  elasticType: ElasticType=0;
  tagDtos: Tag[] = [];
}