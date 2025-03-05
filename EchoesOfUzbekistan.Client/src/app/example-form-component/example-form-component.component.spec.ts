import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExampleFormComponentComponent } from './example-form-component.component';

describe('ExampleFormComponentComponent', () => {
  let component: ExampleFormComponentComponent;
  let fixture: ComponentFixture<ExampleFormComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExampleFormComponentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExampleFormComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
