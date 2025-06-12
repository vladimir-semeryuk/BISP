import { Component, inject } from '@angular/core';
import {
  NavbarComponent,
  NavbarMode,
} from '../../../../shared/components/navbar/navbar.component';
import { FooterComponent } from '../../../../shared/components/footer/footer.component';
import { AuthService } from '../../../../services/auth.service';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton'
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { Observable } from 'rxjs';
import { UserProfileService } from '../../../../services/users/user-profile.service';
import { UserDetails } from '../../../../shared/interfaces/users/UserDetails';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { InfiniteGuideListComponent } from '../../../../shared/components/infinite-guide-list/infinite-guide-list.component';

@Component({
  selector: 'app-d-profile-screen',
  imports: [
    NavbarComponent,
    FooterComponent,
    NzButtonModule,
    NzIconModule,
    CommonModule,
    NzSpinModule,
    InfiniteGuideListComponent,
    NzSkeletonModule
  ],
  templateUrl: './d-profile-screen.component.html',
  styleUrl: './d-profile-screen.component.less',
})
export class DProfileScreenComponent {
  userProfile$!: Observable<UserDetails | null>;
  authService = inject(AuthService);
  navbarMode = NavbarMode.USER;
  isOwnProfile = false;
  currentUserId: string | null = null;
  isLoading = true;

  private userProfileService = inject(UserProfileService);
  private route = inject(ActivatedRoute);

  ngOnInit() {
    // Get current user's ID
    this.userProfileService.getUserProfile().subscribe((profile) => {
      this.currentUserId = profile?.id || null;
    });

    this.route.paramMap.subscribe((params) => {
      const userId = params.get('id'); // Get user ID from route

      if (userId) {
        // If userId exists in the URL, get the specific user's profile
        this.userProfile$ = this.userProfileService.getUserProfileById(userId);
        this.isOwnProfile = userId === this.currentUserId;
      } else {
        // Otherwise, logged-in user's profile is shown
        this.userProfile$ = this.userProfileService.getUserProfile();
        this.isOwnProfile = true;
      }
      this.isLoading = false;
    });
  }
}
