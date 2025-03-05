import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePlaceModalFormComponent } from './create-place-modal-form.component';

describe('CreatePlaceModalFormComponent', () => {
  let component: CreatePlaceModalFormComponent;
  let fixture: ComponentFixture<CreatePlaceModalFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreatePlaceModalFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatePlaceModalFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
