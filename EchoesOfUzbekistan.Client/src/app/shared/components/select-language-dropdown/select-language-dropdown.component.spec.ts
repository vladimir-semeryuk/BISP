import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectLanguageDropdownComponent } from './select-language-dropdown.component';

describe('SelectLanguageDropdownComponent', () => {
  let component: SelectLanguageDropdownComponent;
  let fixture: ComponentFixture<SelectLanguageDropdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SelectLanguageDropdownComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SelectLanguageDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
