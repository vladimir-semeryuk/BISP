import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DSignupScreenComponent } from './d-signup-screen.component';

describe('DSignupScreenComponent', () => {
  let component: DSignupScreenComponent;
  let fixture: ComponentFixture<DSignupScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DSignupScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DSignupScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
