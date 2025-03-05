import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DHomeScreenComponent } from './d-home-screen.component';

describe('DHomeScreenComponent', () => {
  let component: DHomeScreenComponent;
  let fixture: ComponentFixture<DHomeScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DHomeScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DHomeScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
