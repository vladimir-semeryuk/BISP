import { NavLink } from './../../../../shared/interfaces/NavLink';
import { AuthService } from './../../../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { Component, inject, OnInit } from '@angular/core';
import { NavbarComponent, NavbarMode } from '../../../../shared/components/navbar/navbar.component';
import { FooterComponent } from '../../../../shared/components/footer/footer.component';
import {
  ReactiveFormsModule,
  NonNullableFormBuilder,
  Validators,
  FormGroup,
} from '@angular/forms';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { getUserNavLinks } from '../../../../shared/interfaces/NavLink';
import { UserProfileService } from '../../../../services/users/user-profile.service';

@Component({
  selector: 'app-d-login-screen',
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
  ],
  templateUrl: './d-login-screen.component.html',
  styleUrl: './d-login-screen.component.less',
})
export class DLoginScreenComponent implements OnInit {
  navLinks: NavLink[] | null;
  router = inject(Router);
  navbarMode = NavbarMode.USER;
  authService = inject(AuthService);
  profileService = inject(UserProfileService)
  validateForm!: FormGroup; // Declare it but initialize later

  constructor(private fb: NonNullableFormBuilder) {
    this.navLinks = getUserNavLinks(null);
  }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      email: this.fb.control('', [Validators.required]),
      password: this.fb.control('', [Validators.required]),
      remember: this.fb.control(true),
    });
  }

  submitForm(): void {
    console.log('Login submitted', this.validateForm.value);
    if (this.validateForm.valid) {
      this.authService.login(this.validateForm.value).subscribe((response) => {
        this.profileService.refreshUserProfile().subscribe(profile => {
          console.log('User profile updated:', profile);
          // You can now navigate or update the UI after the profile is refreshed
          this.router.navigate(['']);
        })
        // console.log('Login response:', response);
        // console.log(response);
      });
    } else {
      Object.values(this.validateForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
