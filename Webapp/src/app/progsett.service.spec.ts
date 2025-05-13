import { TestBed } from '@angular/core/testing';

import { ProgsettService } from './progsett.service';

describe('ProgsettService', () => {
  let service: ProgsettService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProgsettService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
