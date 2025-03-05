import { TestBed } from '@angular/core/testing';

import { OwnContentGuard } from './own-content.guard';

describe('OwnContentGuard', () => {
  let guard: OwnContentGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(OwnContentGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
