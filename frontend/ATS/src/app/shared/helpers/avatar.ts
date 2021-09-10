import { Applicant } from '../models/applicants/applicant';
import { OnlyApplicant } from '../models/applicants/only-applicant';
import { FullVacancyCandidate } from '../models/vacancy-candidates/full';

import { ShortVacancyCandidateWithApplicant }
  from '../models/vacancy-candidates/short-with-applicant';

const DEFAULT_AVATAR = '../../../../assets/images/defaultAvatar.png';

export function getApplicantAvatar(
  applicant:
  | FullVacancyCandidate 
  | ShortVacancyCandidateWithApplicant
  | OnlyApplicant
  | Applicant,
): string {
  if (applicant.hasOwnProperty('photoLink')) {
    return (applicant as any).photoLink ?? DEFAULT_AVATAR;
  }

  return (applicant as any).applicant.photoLink ?? DEFAULT_AVATAR;
}
