import { ApplicantVacancyInfo } from './applicant-vacancy-info';

export interface Applicant {
  id: string
  firstName: string
  lastName: string
  middleName: string
  email: string
  phone: string
  skype: string

  tags: string[]
  vacancies: ApplicantVacancyInfo[]
}