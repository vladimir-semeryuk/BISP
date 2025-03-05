import { PriceInputComponent } from './../../../../../shared/components/price-input/price-input.component';
import { SelectLanguageDropdownComponent } from './../../../../../shared/components/select-language-dropdown/select-language-dropdown.component';
import { Component, inject } from '@angular/core';
import { NavbarComponent } from '../../../../../shared/components/navbar/navbar.component';
import { AuthService } from '../../../../../services/auth.service';
import {
  NavLink,
  getCmsNavLinks,
  getUserNavLinks,
} from '../../../../../shared/interfaces/NavLink';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzToolTipModule} from 'ng-zorro-antd/tooltip';
import { EditMapComponent } from "../../../cms/edit-map/edit-map.component";

@Component({
  selector: 'app-d-modify-guide-screen',
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
    SelectLanguageDropdownComponent,
    NzToolTipModule,
    PriceInputComponent,
    EditMapComponent
],
  templateUrl: './d-modify-guide-screen.component.html',
  styleUrl: './d-modify-guide-screen.component.less',
})
export class DModifyGuideScreenComponent {
  authService = inject(AuthService);
  navLinks: NavLink[] | null = [];
  guideForm!: FormGroup;

  constructor(private fb: FormBuilder) {
    this.navLinks = getCmsNavLinks(null);
  }

  ngOnInit(): void {
    this.navLinks = this.setNavLinks();
    this.guideForm = this.fb.group({
      title: this.fb.control('', [Validators.required]),
      city: this.fb.control('', [Validators.required]),
      comment: this.fb.control('', [Validators.maxLength(5000)])
    });
  }

  setNavLinks() {
    if (!this.authService.isLoggedIn()) return getCmsNavLinks(null);
    const user = this.authService.getUserAuthDetail();
    if (!user) return getCmsNavLinks(null); // should redirect
    return getCmsNavLinks(user!.id);
  }

  submitForm(): void {
    if (this.guideForm.valid) {
      console.log('submit', this.guideForm.value);
    } else {
      Object.values(this.guideForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
