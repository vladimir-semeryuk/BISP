import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DModifyGuideScreenComponent } from './d-modify-guide-screen.component';

describe('DModifyGuideScreenComponent', () => {
  let component: DModifyGuideScreenComponent;
  let fixture: ComponentFixture<DModifyGuideScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DModifyGuideScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DModifyGuideScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
