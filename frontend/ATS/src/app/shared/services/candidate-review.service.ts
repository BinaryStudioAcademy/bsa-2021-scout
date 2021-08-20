import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class CandidateReviewService {
  public constructor(private readonly http: HttpClientService) {}

  public bulkReview(
    stageId: string,
    candidateId: string,
    data: Record<string, number>,
  ): Observable<void> {
    return this.http.postRequest('/candidateReviews/bulk', {
      stageId,
      candidateId,
      data,
    });
  }
}
