import { TestBed } from '@angular/core/testing';

import { AuthSkipTestsGuard } from './auth--skip-tests.guard';

describe('AuthSkipTestsGuard', () => {
  let guard: AuthSkipTestsGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(AuthSkipTestsGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
