import { TestBed } from '@angular/core/testing';

import { BarberDetailGuard } from './barber-detail.guard';

describe('BarberDetailGuard', () => {
  let guard: BarberDetailGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(BarberDetailGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
