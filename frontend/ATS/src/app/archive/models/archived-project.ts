import { ArchivedEntity } from './archived-entity';
import { ArchivedVacancyShort } from './archived-vacancy-short';

export interface ArchivedProject {
  id: string;
  logo: string;
  name: string;
  description: string;
  teamInfo: string;
  creationDate: Date;
  archivedProjectData: ArchivedEntity;
  vacancies?: ArchivedVacancyShort[];
}