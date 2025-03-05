import { Component, HostListener } from '@angular/core';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-navbar-dropdown',
  imports: [NzDropDownModule, NzIconModule],
  templateUrl: './navbar-dropdown.component.html',
  styleUrl: './navbar-dropdown.component.less',
})
export class NavbarDropdownComponent {
  isMobile: boolean = window.innerWidth < 768;
  dropdownTrigger: 'click' | 'hover' = this.isMobile ? 'click' : 'hover';

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.isMobile = window.innerWidth < 768;
    this.dropdownTrigger = this.isMobile ? 'click' : 'hover';
  }
  selectedLanguage: string = 'EN';

  changeLanguage(lang: string) {
    this.selectedLanguage = lang;
    console.log(`Language changed to: ${lang}`);
    // TODO: Implement actual language switch logic here
  }
}
