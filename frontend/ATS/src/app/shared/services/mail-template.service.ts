import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProjectInfo } from 'src/app/projects/models/project-info';
import { HttpClientService } from '../../shared/services/http-client.service';
import { Applicant } from '../models/applicants/applicant';
import { MailTemplate } from '../models/mail-template/mail-template';
import { MailTemplateCreate } from '../models/mail-template/mail-template-create';
import { MailTemplateTable } from '../models/mail-template/mail-template-table';
import { VacancyData } from '../models/vacancy/vacancy-data';

@Injectable({
  providedIn: 'root',
})
export class MailTemplateService {
  public routePrefix: string = '/mailtemplates';

  constructor(private readonly http: HttpClientService) { }

  public getList() {
    return this.http.getFullRequest<MailTemplateTable[]>(
      `${this.routePrefix}`,
    );
  }

  public getMailTempalte(mailTempalteId: string) {
    return this.http.getRequest<MailTemplate>(
      this.routePrefix + `/${mailTempalteId}`,
    );
  }

  public createMailTempalte(mailTemplate: MailTemplateCreate, files : File[]) {
    const formData = new FormData();
    formData.append('body', JSON.stringify(mailTemplate));
    files.forEach((f) => formData.append('files', f));
    return this.http.postFullRequest<MailTemplateCreate>(`${this.routePrefix}`, formData);
  }

  public updateMailTempalte(mailTemplate: MailTemplate, files : File[]) {
    const formData = new FormData();
    formData.append('body', JSON.stringify(mailTemplate));
    files.forEach((f) => formData.append('files', f));
    return this.http.putFullRequest<MailTemplateCreate>(`${this.routePrefix}`, formData);
  }

  public deleteMailTempalte(mailTemplate: MailTemplateTable) {
    return this.http.deleteFullRequest<MailTemplateTable[]>(
      `${this.routePrefix}/${mailTemplate.id}`,
    );
  }

  public SendEmail(id: string, email: string, vacancy: VacancyData | null | undefined, 
    project: ProjectInfo | null | undefined, applicant: Applicant | null | undefined) {

    if(!vacancy){
      vacancy = null;
    }
    if(!project){
      project = null;
    }
    if(!applicant){
      applicant = null;
    }

    let jsonData: { vacancy: string , project: string , applicant:string} = {
      vacancy: JSON.stringify(vacancy),
      project: JSON.stringify(project),
      applicant: JSON.stringify(applicant),
    };
    
    const formData = new FormData();
    formData.append('body', JSON.stringify(jsonData));
    return this.http.postFullRequest<MailTemplateCreate>(
      `${this.routePrefix}/sendEmail/${id}/${email}`, formData);
  }
  
}