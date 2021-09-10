import { Applicant } from '../models/applicants/applicant';
import { OnlyApplicant } from '../models/applicants/only-applicant';
import { FullVacancyCandidate } from '../models/vacancy-candidates/full';

export function getApplicantAvatar(
  applicant: FullVacancyCandidate | OnlyApplicant | Applicant,
): string {
  return applicant.photoLink ?? '../../../../assets/images/defaultAvatar.png';
}
