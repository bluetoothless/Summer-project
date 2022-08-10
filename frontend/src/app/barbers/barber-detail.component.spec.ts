import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BarberDetailComponent } from './barber-detail.component';

describe('BarberDetailComponent', () => {
  let component: BarberDetailComponent;
  let fixture: ComponentFixture<BarberDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BarberDetailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BarberDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
