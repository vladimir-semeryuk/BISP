import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, NonNullableFormBuilder, Validators, FormBuilder } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { AuthService } from '../../../../services/auth.service';
import { NzResultModule } from 'ng-zorro-antd/result';
import { NavbarComponent, NavbarMode } from '../../../../shared/components/navbar/navbar.component';
import { NavLink, getUserNavLinks } from '../../../../shared/interfaces/NavLink';
import { ChangePasswordRequest } from '../../../../shared/interfaces/auth/change-password-request';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-password-screen',
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
    NzResultModule,
    CommonModule
  ],
  templateUrl: './forgot-password-screen.component.html',
  styleUrl: './forgot-password-screen.component.less'
})
export class ForgotPasswordScreenComponent implements OnInit {
  navLinks: NavLink[] | null;
  router = inject(Router);
  authService = inject(AuthService);
  validateForm!: FormGroup;
  navbarMode = NavbarMode.USER;
  isSuccess: boolean | null = null; // Variable to track success/failure
  errorMessage: string = ''; // Error message to show in case of failure

  constructor(private fb: FormBuilder) {
    this.navLinks = getUserNavLinks(null); // Initialize navigation links
  }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      email: this.fb.control('', [Validators.required, Validators.email]), // Add email validation
      newPassword: this.fb.control('', [Validators.required, Validators.minLength(6)]) // Add password validation
    });
  }

  submitForm(): void {
    if (this.validateForm.valid) {
      // Construct the request payload
      const changePasswordRequest: ChangePasswordRequest = {
        Email: this.validateForm.value.email,
        NewPassword: this.validateForm.value.newPassword
      };

      // Call the AuthService to change the password
      this.authService.changePassword(changePasswordRequest).subscribe(
        (response) => {
          this.isSuccess = true; // Password changed successfully
          this.errorMessage = ''; // Reset any previous error message
          setTimeout(() => this.router.navigate(['']), 2000); // Redirect after a short delay
        },
        (error) => {
          console.error('Error changing password:', error);
          this.isSuccess = false; // Password change failed
          this.errorMessage = 'There was an error changing the password. Please try again.';
        }
      );
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