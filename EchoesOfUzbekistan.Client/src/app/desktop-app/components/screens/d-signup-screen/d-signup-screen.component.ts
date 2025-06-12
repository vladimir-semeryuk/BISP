import { RouterModule } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NavbarComponent, NavbarMode } from '../../../../shared/components/navbar/navbar.component';
import { getUserNavLinks, NavLink } from '../../../../shared/interfaces/NavLink';
import { SelectCountryDropdownComponent } from '../../../../shared/components/select-country-dropdown/select-country-dropdown.component';
import { PriceInputComponent } from "../../../../shared/components/price-input/price-input.component";
import { CommonModule } from '@angular/common';
import { getCustomValidateStatus } from './temporary-util';


@Component({
  selector: 'app-d-signup-screen',
  imports: [
    RouterModule,
    NzIconModule,
    NzDividerModule,
    NavbarComponent,
    ReactiveFormsModule,
    NzButtonModule,
    NzCheckboxModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    SelectCountryDropdownComponent,
    PriceInputComponent,
    CommonModule
],
  templateUrl: './d-signup-screen.component.html',
  styleUrl: './d-signup-screen.component.less',
})
export class DSignupScreenComponent implements OnInit {
  navLinks: NavLink[] | null;
  navbarMode = NavbarMode.USER;
  validateForm!: FormGroup; // Declare it but initialize later

  constructor(private fb: NonNullableFormBuilder) {
    this.navLinks = getUserNavLinks(null)
  }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      firstName: this.fb.control('', [Validators.required]),
      surname: this.fb.control('', [Validators.required]),
      city: this.fb.control('', [Validators.required]),
      email: this.fb.control('', [Validators.required]),
      password: this.fb.control('', [Validators.required]),
      // price: this.fb.control([{}])
    });
  }

  submitForm(): void {
    if (this.validateForm.valid) {
      console.log('submit', this.validateForm.value);
    } else {
      Object.values(this.validateForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  getStatus(control: AbstractControl | null): string {
    return getCustomValidateStatus(control);
  }
}
