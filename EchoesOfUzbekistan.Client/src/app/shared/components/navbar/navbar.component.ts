import { Component, inject, Input, OnInit } from '@angular/core';
import { NavbarDropdownComponent } from '../navbar-dropdown/navbar-dropdown.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { CommonModule } from '@angular/common';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { getCmsNavLinks, getUserNavLinks } from '../../interfaces/NavLink';
import { combineLatest, Subscription } from 'rxjs';
import { UserProfileService } from '../../../services/users/user-profile.service';
import { UserDetails } from '../../interfaces/users/UserDetails';

export enum NavbarMode {
  USER = 'user',
  CMS = 'cms',
}
@Component({
  selector: 'app-navbar',
  imports: [
    NavbarDropdownComponent,
    RouterModule,
    NzButtonModule,
    NzDropDownModule,
    NzIconModule,
    CommonModule,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.less',
})
export class NavbarComponent implements OnInit {
  @Input() mode: NavbarMode = NavbarMode.USER;
  navbarClass = 'navbar user-navbar';
  isCmsMode = false;
  navLinks: { label: string; route: string }[] = [];
  dropdownOpen: boolean = false;

  private userService = inject(UserProfileService);
  private authService = inject(AuthService);
  private subscription: Subscription = Subscription.EMPTY;

  ngOnInit(): void {
    if (this.mode === NavbarMode.CMS) {
      this.navbarClass = 'cms-navbar';
      this.isCmsMode = true;
    }

    // Combine both user profile and auth state to determine navigation links
    this.subscription = combineLatest([
      this.userService.userProfile$,
      this.authService.user$
    ]).subscribe(([profile, authUser]) => {
      // If either profile or authUser is null, treat as not logged in
      const isLoggedIn = profile !== null && authUser !== null;
      const userId = isLoggedIn ? profile?.id : null;
      
      const allLinks = this.isCmsMode
        ? getCmsNavLinks(userId)
        : getUserNavLinks(userId);
      this.navLinks = allLinks;
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  toggleDropdown() {
    this.dropdownOpen = !this.dropdownOpen;
  }

  closeDropdown() {
    this.dropdownOpen = false;
  }

  onLogout(): void {
    this.authService.logout();
  }
}
