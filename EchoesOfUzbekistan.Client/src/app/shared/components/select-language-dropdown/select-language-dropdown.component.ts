import { Component, forwardRef, inject } from '@angular/core';
import { Validators, ReactiveFormsModule, NG_VALUE_ACCESSOR, FormControl, ControlContainer } from '@angular/forms';
import { Language } from '../../interfaces/common/Language';
import { LanguageService } from '../../../services/language/language.service';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { SelectOptionDropdownComponent } from '../abstractions/select-option-dropdown';

@Component({
  selector: 'app-select-language-dropdown',
  imports: [NzSelectModule, ReactiveFormsModule],
  templateUrl: './select-language-dropdown.component.html',
  styleUrl: './select-language-dropdown.component.less',
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => inject(ControlContainer, {skipSelf: true})
    }
      ],
  // providers: [
  //   {
  //     provide: NG_VALUE_ACCESSOR,
  //     useExisting: forwardRef(() => SelectLanguageDropdownComponent),
  //     multi: true
  //   }]
})
export class SelectLanguageDropdownComponent extends SelectOptionDropdownComponent<Language> {

  constructor(private languageService: LanguageService) {
      super();
    }
  
    override loadOptions(): void {
      this.languageService.getLanguages().subscribe({
        next: (languages) => {
          this.options = languages;
        },
        error: (err) => console.error('Error loading languages:', err)
      });
    }
    override setValidators(required: boolean, formControl: FormControl<Language | null>): void {
      if (required) {
        formControl.setValidators([Validators.required]);
      }
      formControl.updateValueAndValidity();
    }
    
  // constructor(private languageService: LanguageService) {
  //   super();
  // }

  // override loadOptions(): void {
  //   this.languageService.getLanguages().subscribe({
  //     next: (langs) => {
  //       // console.log('Languages loaded:', langs);
  //       this.options = langs;
  //     },
  //     error: (err) => {
  //       console.error('Error loading languages:', err);
  //     }
  //   });
  // }

  // override setValidators(required: boolean, formControl: FormControl<Language | null>): void {
  //   if (required) {
  //     formControl.setValidators([Validators.required]);
  //   }
  //   formControl.updateValueAndValidity();
  // }
}
