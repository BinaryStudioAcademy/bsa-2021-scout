import { CreateApplicant } from '../models/applicants/create-applicant';
import { UpdateApplicant } from '../models/applicants/update-applicant';

export function getModifyApplicantBody(dto: CreateApplicant | UpdateApplicant): any {
  return {
    ...dto,
    cvLink: typeof dto.cv === 'string' ? dto.cv : null,
    photoLink: typeof dto.photo === 'string' ? dto.photo : null,
  };
}

export function getModifyApplicantFormData(dto: CreateApplicant | UpdateApplicant): FormData {
  const formData = new FormData();
  formData.append('body', JSON.stringify(getModifyApplicantBody(dto)));

  if (dto.cv && typeof dto.cv !== 'string') {
    formData.append('cvFile', dto.cv);
  }

  if (dto.photo && typeof dto.photo !== 'string') {
    formData.append('photoFile', dto.photo);
  }

  return formData;
}
