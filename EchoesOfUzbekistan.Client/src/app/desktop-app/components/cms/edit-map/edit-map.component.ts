import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnDestroy,
} from '@angular/core';
import { GeolocateControl, Map, Marker, NavigationControl, Popup } from 'maplibre-gl';
import { MaplibreTerradrawControl } from '@watergis/maplibre-gl-terradraw';
import { environment } from '../../../../../environments/environment.development';

@Component({
  selector: 'app-edit-map',
  imports: [],
  templateUrl: './edit-map.component.html',
  styleUrl: './edit-map.component.less',
})
export class EditMapComponent implements OnInit, AfterViewInit, OnDestroy {
  map!: maplibregl.Map;
  draw!: MaplibreTerradrawControl;
  selectedCoordinates: [number, number] | null = null;
  showModal = false;
  placeName = '';

  @ViewChild('map')
  private mapContainer!: ElementRef<HTMLElement>;

  ngOnInit(): void {}
  ngAfterViewInit(): void {
    this.initializeMap();
    //     const draw = new MaplibreTerradrawControl({
    //       modes: [
    //         'render',
    //         'point',
    //         'linestring',
    //         'delete'
    //       ],
    //       open: false,
    //     });

    //     this.map.addControl(draw, 'top-right');

        this.map.addControl(
          new GeolocateControl({
              positionOptions: {
                  enableHighAccuracy: true
              },
              showAccuracyCircle: false,
              trackUserLocation: true
          })
    );
  }

  initializeMap(): void {
    const initialState = { lng: 69.28, lat: 41.311, zoom: 14 };

    this.map = new Map({
      container: this.mapContainer.nativeElement,
      style: `https://api.maptiler.com/maps/streets-v2/style.json?key=${environment.maptilerApiKey}`,
      center: [initialState.lng, initialState.lat],
      zoom: initialState.zoom,
      rollEnabled: true,
    });

  //   this.map.addControl(new NavigationControl({
  //     visualizePitch: true,
  //     visualizeRoll: true,
  //     showZoom: true,
  //     showCompass: true
  // }));

    this.draw = new MaplibreTerradrawControl({
      modes: ['render', 'point', 'linestring', 'delete'],
      open: false,
    });

    this.map.addControl(this.draw, 'top-right');

    const instance = this.draw.getTerraDrawInstance();

    instance.on('change', (ids: any[], type: string) => {
      if (type === 'create') {
        if (instance.getMode() !== 'point'){
          return
        }
        // Get all features from TerraDrawControl
        const allFeatures = this.draw.getFeatures()?.features ?? [];
        // Find the created features by matching IDs
        const createdFeatures = allFeatures.filter((f) => ids.includes(f.id));

        // Check if any created feature is a Point
        const createdPoints = createdFeatures.filter(
          (f) => f.geometry.type === 'Point'
        );

        if (createdPoints.length > 0) {
          const newPoint = createdPoints[0];
          const coords = newPoint.geometry.coordinates;
          if (Array.isArray(coords) && typeof coords[0] === "number" && typeof coords[1] === "number") {
            this.selectedCoordinates = coords as [number, number]; // Safely cast it
            console.log(this.selectedCoordinates)
            this.placeName = "New place"
            this.savePlace()
          }
        }
      }
    });
  }

  // Handle form submission
  savePlace(): void {
    if (this.selectedCoordinates && this.placeName) {
      new Marker({ color: 'red' })
        .setLngLat(this.selectedCoordinates)
        .setPopup(new Popup().setText(this.placeName))
        .addTo(this.map!);

      this.showModal = false;
      this.placeName = '';
    }
  }

  ngOnDestroy(): void {
    this.map?.remove();
  }
}
