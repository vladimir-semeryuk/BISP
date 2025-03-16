import { Injectable, ComponentRef, Type, ViewContainerRef } from '@angular/core';
import maplibregl, { Popup } from 'maplibre-gl';

@Injectable({
  providedIn: 'root'
})
export class PopupService {
  private activePopups: ComponentRef<any>[] = []; 
  openPopup<T extends object>(
    map: maplibregl.Map,
    lng: number,
    lat: number,
    component: Type<T>,
    viewContainerRef: ViewContainerRef,
    props: Partial<T> = {}
  ): maplibregl.Popup {

  const componentRef = viewContainerRef.createComponent(component);
  Object.assign(componentRef.instance, props);

  this.activePopups.push(componentRef);

  const element = (componentRef.hostView as any).rootNodes[0];

  const popup = new Popup().setDOMContent(element).setLngLat([lng, lat]).addTo(map);

  return popup;
  }
}