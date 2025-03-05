import { NavLink } from './../../../../shared/interfaces/NavLink';
import { AuthService } from './../../../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { Component, inject, OnInit } from '@angular/core';
import { NavbarComponent } from '../../../../shared/components/navbar/navbar.component';
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
  router = inject(Router)
  authService = inject(AuthService)
  validateForm!: FormGroup; // Declare it but initialize later

  constructor(private fb: NonNullableFormBuilder) {
    this.navLinks = getUserNavLinks(null)
  }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      email: this.fb.control('', [Validators.required]),
      password: this.fb.control('', [Validators.required]),
      remember: this.fb.control(true),
    });
  }

  submitForm(): void {
    if (this.validateForm.valid) {
      this.authService.login(this.validateForm.value).subscribe((response) => {
        console.log(response);
        this.router.navigate(['']);
      })
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
