import { Component, inject } from '@angular/core';
import { NavbarComponent } from "../../../../shared/components/navbar/navbar.component";
import { FooterComponent } from "../../../../shared/components/footer/footer.component";
import { AuthService } from '../../../../services/auth.service';
import { NavLink, getUserNavLinks } from '../../../../shared/interfaces/NavLink';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-d-profile-screen',
  imports: [NavbarComponent, FooterComponent, NzButtonModule, NzIconModule],
  templateUrl: './d-profile-screen.component.html',
  styleUrl: './d-profile-screen.component.less'
})
export class DProfileScreenComponent {
  authService = inject(AuthService)
    navLinks: NavLink[] | null = [];
  
    ngOnInit() {
      this.navLinks = this.setNavLinks();
    }
  
    setNavLinks(){
      if (!this.authService.isLoggedIn()) return getUserNavLinks(null)
      const user = this.authService.getUserAuthDetail()
      if (!user)
        return getUserNavLinks(null);
      return getUserNavLinks(user!.id)
    }
}
