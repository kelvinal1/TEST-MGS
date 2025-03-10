import { TestBed } from '@angular/core/testing';

import { InvoiceGeneralService } from './invoice-general.service';

describe('InvoiceGeneralService', () => {
  let service: InvoiceGeneralService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InvoiceGeneralService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
