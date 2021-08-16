import { TestBed } from '@angular/core/testing';

import { VacancyDataService } from './vacancy-data.service';

describe('VacancyDataService', () => {
  let service: VacancyDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VacancyDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
