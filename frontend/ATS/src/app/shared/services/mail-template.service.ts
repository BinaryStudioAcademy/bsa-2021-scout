import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../../shared/services/http-client.service';
import { MailTemplateCreate } from '../models/mail-template/mail-template-create';
import { MailTemplateTable } from '../models/mail-template/mail-template-table';

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

  public createMailTempalte(mailTemplate: MailTemplateCreate, files : File[]) {
    const formData = new FormData();
    formData.append('body', JSON.stringify(mailTemplate));
    files.forEach((file :File) => {
      formData.append(file.name, file);
    });
    return this.http.postFullRequest<MailTemplateCreate>(`${this.routePrefix}`, formData);
  }

  public deleteMailTempalte(mailTemplate: MailTemplateTable) {
    return this.http.deleteFullRequest<MailTemplateTable[]>(
      `${this.routePrefix}/${mailTemplate.id}`,
    );
  }
  
}