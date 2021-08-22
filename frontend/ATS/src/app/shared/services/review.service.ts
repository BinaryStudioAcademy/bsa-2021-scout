import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Review } from '../models/reviews/review';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  public constructor(private readonly http: HttpClientService) {}

  public getAll(): Observable<Review[]> {
    return this.http.getRequest('/reviews');
  }
}
