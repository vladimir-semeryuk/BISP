import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DCmsHomeScreenComponent } from './d-cms-home-screen.component';

describe('DCmsHomeScreenComponent', () => {
  let component: DCmsHomeScreenComponent;
  let fixture: ComponentFixture<DCmsHomeScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DCmsHomeScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DCmsHomeScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
