import { Injectable } from '@angular/core';
import { HttpClientService } from 'src/app/shared/services/http-client.service';
import { CreateInterviewDto } from '../models/create-interview-dto.model';
import { Interview } from '../models/interview.model';

@Injectable({
  providedIn: 'root',
})
export class InterviewsService {
  public routePrefix = '/interviews';

  constructor(private httpService: HttpClientService) { }

  public getInterviews() {
    return this.httpService.getFullRequest<Interview[]>(`${this.routePrefix}`);
  }

  public getInterview(id: number) {
    return this.httpService.getFullRequest<Interview>(`${this.routePrefix}/${id}`);
  }

  public createInterview(post: CreateInterviewDto) {
    return this.httpService.postFullRequest<CreateInterviewDto>(`${this.routePrefix}`, post);
  }

  public deleteInterview(interview: Interview) {
    return this.httpService.deleteFullRequest<Interview>(`${this.routePrefix}/${interview.id}`);
  }

  public updateInterview(interview: CreateInterviewDto) {
    return this.httpService.putFullRequest<CreateInterviewDto>(`${this.routePrefix}`, interview);
  }
}
