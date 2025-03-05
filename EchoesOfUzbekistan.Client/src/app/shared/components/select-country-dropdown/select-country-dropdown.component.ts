import { CountryService } from './../../../services/country/country.service';
import { Component, inject, } from '@angular/core';
import { ReactiveFormsModule, ControlContainer, FormControl, Validators } from '@angular/forms';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { Country } from '../../interfaces/common/country';
import { SelectOptionDropdownComponent } from '../abstractions/select-option-dropdown';

@Component({
  selector: 'app-select-country-dropdown',
  imports: [NzSelectModule, ReactiveFormsModule],
  templateUrl: './select-country-dropdown.component.html',
  styleUrl: './select-country-dropdown.component.less',
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => inject(ControlContainer, {skipSelf: true})
    }
      ]
})
export class SelectCountryDropdownComponent extends SelectOptionDropdownComponent<Country> {
  constructor(private countryService: CountryService) {
    super();
  }

  override loadOptions(): void {
    this.countryService.getCountries().subscribe({
      next: (countries) => (this.options = countries),
      error: (err) => console.error('Error loading countries:', err)
    });
  }
  override setValidators(required: boolean, formControl: FormControl<Country | null>): void {
    if (required) {
      formControl.setValidators([Validators.required]);
    }
    formControl.updateValueAndValidity();
  }
  
}
