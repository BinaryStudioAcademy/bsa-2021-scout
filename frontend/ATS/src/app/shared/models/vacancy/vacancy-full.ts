import { ElasticEntity } from '../elastic-entity/elastic-entity';
import { Stage } from '../stages/stage';
import { VacancyStatus } from './vacancy-status';

export class VacancyFull {
  title: string = '';
  description: string = '';
  requirements: string = '';
  projectId: string = '';
  salaryFrom: number = 0;
  salaryTo: number = 0;
  tierFrom: number = 0;
  tierTo: number = 0;
  sources: string = '';
  isHot: boolean = false;
  isRemote: boolean = false;
  stages: Stage[] = [];
  responsibleHrId: string = '';
  companyId: string = '';
  status: VacancyStatus = 0;
  creationDate: Date = new Date();
  dateOfOpening: Date = new Date();
  modificationDate: Date = new Date();
  completionDate: Date = new Date();
  plannedCompletionDate: Date = new Date();
  tags: ElasticEntity = new ElasticEntity();
}