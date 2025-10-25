import { TestBed } from '@angular/core/testing';

import { TestApi } from './test-api';

describe('TestApi', () => {
  let service: TestApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TestApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
