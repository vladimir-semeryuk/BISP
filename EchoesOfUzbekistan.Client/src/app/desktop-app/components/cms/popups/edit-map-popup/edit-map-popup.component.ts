import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Popup } from 'maplibre-gl';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-map-popup',
  templateUrl: './edit-map-popup.component.html',
  styleUrl: './edit-map-popup.component.less',
  imports: [CommonModule, NzButtonModule, NzIconModule]
})
export class MapPopupComponent {
  @Input() title!: string;
  @Input() description!: string;
  @Input() imageUrl:string | null = null;
  popupRef?: Popup;

  closePopup() {
    this.popupRef?.remove();
  }
}