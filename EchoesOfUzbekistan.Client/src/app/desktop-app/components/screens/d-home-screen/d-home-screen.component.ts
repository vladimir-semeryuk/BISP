import { RouterModule } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { Component, inject, OnInit } from '@angular/core';
import { NavbarComponent } from "../../../../shared/components/navbar/navbar.component";
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { CommonModule } from '@angular/common';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzInputModule } from 'ng-zorro-antd/input';
import { FooterComponent } from "../../../../shared/components/footer/footer.component";
import { getUserNavLinks, NavLink } from '../../../../shared/interfaces/NavLink';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-d-home-screen',
  imports: [NavbarComponent, RouterModule, FooterComponent, NzButtonModule, NzIconModule, NzInputModule, NzCardModule, CommonModule, NzGridModule, NzDividerModule, FooterComponent, NavbarComponent],
  templateUrl: './d-home-screen.component.html',
  styleUrl: './d-home-screen.component.less'
})
export class DHomeScreenComponent implements OnInit {
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

  topItems = [
    {
      title: "Magical Chilonzor",
      image: "https://images.unsplash.com/photo-1719144065955-89a4dadaba41?ixlib=rb-4.0.3&q=85&fm=jpg&crop=entropy&cs=srgb&dl=alessio-roversi-IFlbZJgP2S8-unsplash.jpg&w=1920",
      subtitle: "Explore the most populated...",
      description: "Lorem ipsum dolor sit amet, cons..."
    },
    {
      title: "Ancient Samarkand",
      image: "../../../../../../public/home-bg.jpg",
      subtitle: "Discover the blue domes...",
      description: "A city of breathtaking architecture..."
    },
    {
      title: "Magical Chilonzor",
      image: "../../../../../../public/home-bg.jpg",
      subtitle: "Explore the most populated...",
      description: "Lorem ipsum dolor sit amet, cons..."
    },
    {
      title: "Ancient Samarkand",
      image: "assets/images/samarkand.jpg",
      subtitle: "Discover the blue domes...",
      description: "A city of breathtaking architecture..."
    },
    {
      title: "Magical Chilonzor",
      image: "assets/images/chilonzor.jpg",
      subtitle: "Explore the most populated...",
      description: "Lorem ipsum dolor sit amet, cons..."
    },
    {
      title: "Ancient Samarkand",
      image: "assets/images/samarkand.jpg",
      subtitle: "Discover the blue domes...",
      description: "A city of breathtaking architecture..."
    },
    {
      title: "Magical Chilonzor",
      image: "assets/images/chilonzor.jpg",
      subtitle: "Explore the most populated...",
      description: "Lorem ipsum dolor sit amet, cons..."
    },
    {
      title: "Ancient Samarkand",
      image: "assets/images/samarkand.jpg",
      subtitle: "Discover the blue domes...",
      description: "A city of breathtaking architecture..."
    }
  ];
}
