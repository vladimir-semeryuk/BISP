import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DViewSingleGuideScreenComponent } from './d-view-single-guide-screen.component';

describe('DViewSingleGuideScreenComponent', () => {
  let component: DViewSingleGuideScreenComponent;
  let fixture: ComponentFixture<DViewSingleGuideScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DViewSingleGuideScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DViewSingleGuideScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
