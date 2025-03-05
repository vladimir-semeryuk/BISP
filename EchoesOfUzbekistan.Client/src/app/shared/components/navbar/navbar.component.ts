import { Component, HostListener, Input, OnInit } from '@angular/core';
import { NavbarDropdownComponent } from "../navbar-dropdown/navbar-dropdown.component";
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { CommonModule } from '@angular/common';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-navbar',
  imports: [NavbarDropdownComponent, 
    RouterModule,
    NzButtonModule,
    NzDropDownModule,
    NzIconModule,
    CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.less'
})
export class NavbarComponent {
  @Input() navLinks: { label: string; route: string }[] | null = [];
  @Input() navbarClass: string = 'user-navbar';

  dropdownOpen: boolean = false;

  toggleDropdown() {
    this.dropdownOpen = !this.dropdownOpen;
  }

  closeDropdown() {
    this.dropdownOpen = false;
}
}