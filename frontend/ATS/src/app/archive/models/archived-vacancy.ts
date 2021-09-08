import { User } from 'src/app/users/models/user';
import { ArchivedEntity } from './archived-entity';

export interface ArchivedVacancy {
  id: string;
  title: string;
  projectName: string;
  creationDate: Date;
  completionDate?: Date;
  responsibleHr: User;
  archivedVacancyData: ArchivedEntity;
  isProjectArchived: boolean;
}