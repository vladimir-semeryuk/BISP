import { Component, inject } from '@angular/core';
import { NavbarComponent } from "../../../../shared/components/navbar/navbar.component";
import { FooterComponent } from "../../../../shared/components/footer/footer.component";
import { AuthService } from '../../../../services/auth.service';
import { NavLink, getUserNavLinks } from '../../../../shared/interfaces/NavLink';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { Observable } from 'rxjs';
import { UserProfileService } from '../../../../services/users/user-profile.service';
import { UserDetails } from '../../../../shared/interfaces/users/UserDetails';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-d-profile-screen',
  imports: [NavbarComponent, FooterComponent, NzButtonModule, NzIconModule, CommonModule],
  templateUrl: './d-profile-screen.component.html',
  styleUrl: './d-profile-screen.component.less'
})
export class DProfileScreenComponent {
  userProfile$!: Observable<UserDetails | null>;
  authService = inject(AuthService)

  private userProfileService = inject(UserProfileService);
  private route = inject(ActivatedRoute);

    navLinks: NavLink[] | null = [];
  
    ngOnInit() {
      this.navLinks = this.setNavLinks();
      this.route.paramMap.subscribe(params => {
        const userId = params.get('id'); // Get user ID from route
  
        if (userId) {
          // If userId exists in the URL, get the specific user's profile 
          this.userProfile$ = this.userProfileService.getUserProfileById(userId);
        } else {
          // Otherwise, show the logged-in user's profile (cached)
          this.userProfile$ = this.userProfileService.getUserProfile();
        }
      });
    }
  
    setNavLinks(){
      if (!this.authService.isLoggedIn()) return getUserNavLinks(null)
      const user = this.authService.getUserAuthDetail()
      if (!user)
        return getUserNavLinks(null);
      return getUserNavLinks(user!.id)
    }
}
