import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DProfileScreenComponent } from './d-profile-screen.component';

describe('DProfileScreenComponent', () => {
  let component: DProfileScreenComponent;
  let fixture: ComponentFixture<DProfileScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DProfileScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DProfileScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
