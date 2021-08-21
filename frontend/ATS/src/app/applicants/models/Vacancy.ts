import { VacancyStatus } from 'src/app/shared/models/vacancy/vacancy-status';
import { User } from 'src/app/users/models/user';

export class Vacancy {
  id: string='';
  title: string='';
  currentApplicantsAmount: number=0;
  requiredCandidatesAmount: number=0;
  department: string='';
  responsibleHr!: User;
  creationDate!: Date;
  status!: VacancyStatus;
}
    ​