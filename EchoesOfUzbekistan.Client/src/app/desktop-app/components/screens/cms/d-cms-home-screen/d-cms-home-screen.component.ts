import { Component, inject } from '@angular/core';
import {
  NavbarComponent,
  NavbarMode,
} from '../../../../../shared/components/navbar/navbar.component';
import { FooterComponent } from '../../../../../shared/components/footer/footer.component';
import { NzCardModule } from 'ng-zorro-antd/card';
import { AuthService } from '../../../../../services/auth.service';
import { GuideService } from '../../../../../services/guides/guide.service';
import { UserProfileService } from '../../../../../services/users/user-profile.service';
import {
  NavLink,
  getCmsNavLinks,
} from '../../../../../shared/interfaces/NavLink';
import { GuideDto } from '../../../../../shared/interfaces/guides/guide-dto';
import { CommonModule } from '@angular/common';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzButtonModule } from 'ng-zorro-antd/button';

@Component({
  selector: 'app-d-cms-home-screen',
  imports: [
    NavbarComponent,
    FooterComponent,
    NzCardModule,
    CommonModule,
    NzIconModule,
    NzButtonModule,
  ],
  templateUrl: './d-cms-home-screen.component.html',
  styleUrl: './d-cms-home-screen.component.less',
})
export class DCmsHomeScreenComponent {
  authService = inject(AuthService);
  currentUserId: string = '';
  navLinks: NavLink[] | null = [];
  navbarMode = NavbarMode.CMS;
  guides: GuideDto[] = [];

  constructor(
    private guideService: GuideService,
    private userService: UserProfileService
  ) {
    this.navLinks = getCmsNavLinks(null);
  }

  ngOnInit(): void {

    this.userService.getUserProfile().subscribe((t) => {
      if (t?.id) {
        this.currentUserId = t.id;

        this.guideService
          .getGuides({ createdByUserId: this.currentUserId })
          .subscribe((guides) => {
            console.log('User guides:', guides);
            this.guides = guides;
          });
      }
    });
  }

  // Settings Button Click
  onSettings(guide: GuideDto): void {
    console.log('Settings for guide:', guide);
  }

  // Edit Button Click
  onEdit(guide: GuideDto): void {
    console.log('Edit guide:', guide);
  }

  // More Button Click
  onMore(guide: GuideDto): void {
    console.log('More options for guide:', guide);
  }
}
