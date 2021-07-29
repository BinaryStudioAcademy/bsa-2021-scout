import { Injectable } from '@angular/core';
import moment from 'moment';
import { Observable, of } from 'rxjs';
import { ShortCandidate } from '../models/candidates/short';
import { HttpClientService } from './http-client.service';
import { CandidateStatus } from '../models/candidates/status';

@Injectable({
  providedIn: 'root',
})
export class CandidateService {
  public constructor(private readonly http: HttpClientService) {}

  public getShortCandidates(): Observable<ShortCandidate[]> {
    return of([
      {
        id: 'id1',
        avatar:
          'https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png',
        fullName: 'Cristofer Westervelt',
        status: CandidateStatus.Applied,
        appliedAt: moment().days(-10)
          .toDate(),
      },
      {
        id: 'id2',
        avatar:
          'https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png',
        fullName: 'Marley Scheifer',
        status: CandidateStatus.PhoneScreen,
        appliedAt: moment().days(-9)
          .toDate(),
      },
      {
        id: 'id3',
        avatar:
          'https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png',
        fullName: 'Lindsey Siphron',
        status: CandidateStatus.Interview,
        appliedAt: moment().days(-8)
          .toDate(),
      },
      {
        id: 'id4',
        avatar:
          'https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png',
        fullName: 'Leo Stanton',
        status: CandidateStatus.Offer,
        appliedAt: moment().days(-2)
          .toDate(),
      },
      {
        id: 'id5',
        avatar:
          'https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png',
        fullName: 'Livia Aminoff',
        status: CandidateStatus.Hired,
        appliedAt: moment().days(-2)
          .toDate(),
      },
      {
        id: 'id6',
        avatar:
          'https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png',
        fullName: 'Livia Aminoff',
        status: CandidateStatus.Test,
        appliedAt: moment().days(-2)
          .toDate(),
      },
    ]);
  }
}
