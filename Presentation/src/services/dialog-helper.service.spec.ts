import { TestBed } from '@angular/core/testing';

import { DialogHelperService } from './dialog-helper.service';

describe('DialogHelperService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DialogHelperService = TestBed.get(DialogHelperService);
    expect(service).toBeTruthy();
  });
});
