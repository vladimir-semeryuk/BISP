import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnDestroy,
  ViewContainerRef,
} from '@angular/core';
import { GeolocateControl, Map, Marker, NavigationControl } from 'maplibre-gl';
import { MaplibreTerradrawControl } from '@watergis/maplibre-gl-terradraw';
import { environment } from '../../../../../environments/environment.development';
import { LineString, Point } from 'geojson';
import { CreatePlaceModalFormComponent } from '../modals/create-place-modal-form/create-place-modal-form.component';
import { UserProfileService } from '../../../../services/users/user-profile.service';
import { PopupService } from '../../../../services/popups/popup.service';
import { MapPopupComponent } from '../popups/edit-map-popup/edit-map-popup.component';
import { PlaceNewlyCreatedDetails } from '../../../../shared/interfaces/places/place-newly-created';

@Component({
  selector: 'app-edit-map',
  imports: [CreatePlaceModalFormComponent],
  templateUrl: './edit-map.component.html',
  styleUrl: './edit-map.component.less',
})
export class EditMapComponent implements OnInit, AfterViewInit, OnDestroy {
  map!: maplibregl.Map;
  draw!: MaplibreTerradrawControl;

  currentUserId: string = '';

  createdPlaces: PlaceNewlyCreatedDetails[] = [];
  pointsToAdd: Point[] = [];
  routeToAdd: LineString[] = [];
  lastCreatedPointId: string | null = null;
  selectedCoordinates: [number, number] | null = null;
  showModal = false;
  placeName = '';

  @ViewChild('popupContainer', { read: ViewContainerRef, static: false })
  popupContainer!: ViewContainerRef;
  @ViewChild(CreatePlaceModalFormComponent)
  createPlaceModal!: CreatePlaceModalFormComponent;
  @ViewChild('map')
  private mapContainer!: ElementRef<HTMLElement>;

  constructor(
    private userService: UserProfileService,
    private popupService: PopupService
  ) {}

  ngOnInit(): void {
    this.userService.getUserProfile().subscribe((t) => {
      if (t?.id) {
        this.currentUserId = t.id;
      }
    });
  }

  ngAfterViewInit(): void {
    this.initializeMap();

    this.map.addControl(
      new GeolocateControl({
        positionOptions: {
          enableHighAccuracy: true,
        },
        showAccuracyCircle: false,
        trackUserLocation: true,
      })
    );

    console.log('ViewContainerRef initialized:', this.popupContainer);
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

    this.map.addControl(
      new NavigationControl({
        visualizePitch: true,
        visualizeRoll: true,
        showZoom: true,
        showCompass: true,
      })
    );

    this.draw = new MaplibreTerradrawControl({
      modes: ['render', 'point', 'linestring', 'delete'],
      open: false,
    });

    this.map.addControl(this.draw, 'top-right');

    const instance = this.draw.getTerraDrawInstance();

    // FOR TEST PURPOSES, SHOULD BE REMOVED LATER ON
    const popup = this.popupService.openPopup(
      this.map,
      69.28,
      41.311,
      MapPopupComponent,
      this.popupContainer,
      {
        title: 'HIOJAKJ',
        description:
          'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi iaculis, magna sit amet gravida porttitor, justo velit tincidunt mi, et molestie nunc lacus sed mi. Morbi iaculis, magna sit amet gravida porttitor, justo velit tincidunt mi, et molestie nunc lacus sed mi.',
      }
    ); 
    new Marker({ color: 'red' })
      .setLngLat([69.28, 41.311])
      .setPopup(popup)
      .addTo(this.map!);

    instance.on('change', (ids: any[], type: string) => {
      if (type === 'create') {
        if (instance.getMode() !== 'point') {
          return;
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
          this.lastCreatedPointId = newPoint.id as string;
          const coords = newPoint.geometry.coordinates;
          if (
            Array.isArray(coords) &&
            typeof coords[0] === 'number' &&
            typeof coords[1] === 'number'
          ) {
            this.selectedCoordinates = coords as [number, number]; // Safely cast it
            console.log(
              `Selected coordinates are equal to ${this.selectedCoordinates}`
            );

            // this.pointsToAdd.push(newPoint.geometry as Point)
            // console.log(this.pointsToAdd)
            // this.placeName = "New place"
            // this.savePlace()
            this.showModal = true;
            this.createPlaceModal.showModal(
              this.selectedCoordinates[0],
              this.selectedCoordinates[1]
            );
          }
        }
      }
    });
  }

  // Handle form submission
  onPlaceSaved(place: PlaceNewlyCreatedDetails): void {
    if (!this.selectedCoordinates) return;

    this.createdPlaces.push(place);
    const popup = this.popupService.openPopup(
      this.map,
      this.selectedCoordinates[0],
      this.selectedCoordinates[1],
      MapPopupComponent,
      this.popupContainer,
      {
        title: place.title,
        description: place.description,
      }
    );
    new Marker({ color: 'red' })
      .setLngLat(this.selectedCoordinates)
      .setPopup(popup)
      .addTo(this.map!);

    console.log('Show Modal set to false');
    this.showModal = false;
    this.selectedCoordinates = null;
  }

  onModalClosed(): void {
    console.log('Modal closed without saving');
    this.deleteLastPoint();
  }

  deleteLastPoint(): void {
    if (this.lastCreatedPointId) {
      console.log(`Deleting point with ID: ${this.lastCreatedPointId}`);
      const instance = this.draw.getTerraDrawInstance();
      instance.removeFeatures([this.lastCreatedPointId]);
      this.lastCreatedPointId = null; // Reset stored point ID
    }
  }

  ngOnDestroy(): void {
    this.map?.remove();
  }
}
