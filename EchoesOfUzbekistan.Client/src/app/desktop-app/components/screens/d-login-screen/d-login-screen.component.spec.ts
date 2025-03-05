import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DLoginScreenComponent } from './d-login-screen.component';

describe('DLoginScreenComponent', () => {
  let component: DLoginScreenComponent;
  let fixture: ComponentFixture<DLoginScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DLoginScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DLoginScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
