import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectCountryDropdownComponent } from './select-country-dropdown.component';

describe('SelectCountryDropdownComponent', () => {
  let component: SelectCountryDropdownComponent;
  let fixture: ComponentFixture<SelectCountryDropdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SelectCountryDropdownComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SelectCountryDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
