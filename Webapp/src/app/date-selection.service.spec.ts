import { TestBed } from '@angular/core/testing';

import { DateSelectionService } from './date-selection.service';

describe('DateSelectionService', () => {
  let service: DateSelectionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DateSelectionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
