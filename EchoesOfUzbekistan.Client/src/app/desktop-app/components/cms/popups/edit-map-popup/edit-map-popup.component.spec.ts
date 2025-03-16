import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditMapPopupComponent } from './edit-map-popup.component';

describe('EditMapPopupComponent', () => {
  let component: EditMapPopupComponent;
  let fixture: ComponentFixture<EditMapPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditMapPopupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditMapPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
