import { Component, ElementRef, inject, ViewChild, OnInit } from '@angular/core';
import {
  NavbarComponent,
  NavbarMode,
} from '../../../../shared/components/navbar/navbar.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzImageModule } from 'ng-zorro-antd/image';
import { NzCommentModule } from 'ng-zorro-antd/comment';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { LngLatBounds, Map, Marker, NavigationControl, Popup } from 'maplibre-gl';
import { environment } from '../../../../../environments/environment.development';
import { CommonModule, DatePipe, formatDate } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { FooterComponent } from '../../../../shared/components/footer/footer.component';
import { GuideDetailsDto } from '../../../../shared/interfaces/guides/guide-details-dto';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { GuideService } from '../../../../services/guides/guide.service';
import { catchError, finalize, of } from 'rxjs';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { CommentService } from '../../../../services/comments/comments.service';
import { CommentDto } from '../../../../shared/interfaces/comments/comment-dto';
import { LikeService } from '../../../../services/likes/like.service';
import { AuthService } from '../../../../services/auth.service';
import { AudioPlayerComponent } from '../../../../shared/components/audio-player/audio-player.component';

@Component({
  selector: 'app-d-view-single-guide-screen',
  imports: [
    NavbarComponent,
    NzFormModule,
    ReactiveFormsModule,
    NzToolTipModule,
    NzButtonModule,
    NzImageModule,
    NzIconModule,
    NzCommentModule,
    CommonModule,
    NzAvatarModule,
    NzInputModule,
    NzSpinModule,
    NzEmptyModule,
    RouterModule,
    FooterComponent,
    AudioPlayerComponent
  ],
  templateUrl: './d-view-single-guide-screen.component.html',
  styleUrl: './d-view-single-guide-screen.component.less',
})
export class DViewSingleGuideScreenComponent implements OnInit {
  // Navbar Mode
  navbarMode = NavbarMode.USER;

  // Guide 
  private guideService = inject(GuideService);
  guide?: GuideDetailsDto;
  isGuideNotFound = false;
  isGuideLoading = true;

  // Likes
  private likeService = inject(LikeService);
  hasLiked = false;
  likesCount = 0;
  isLikeLoading = false;
  isLiked = false;
  isLoadingLike = false;

  // Map Fields
  map!: maplibregl.Map;
  @ViewChild('map')
  private mapContainer!: ElementRef<HTMLElement>;
  private isMapInitialised = false;
  private markers: Marker[] = [];
  private initialCenter: [number, number] = [69.28, 41.311];

  // Comment Form Field
  commentForm: FormGroup;

  currentUser = {
    name: 'Han Solo',
    avatar: '//zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png',
  };

  // Comments
  newComment = '';
  private commentService = inject(CommentService);
  comments: CommentDto[] = [];
  isLoadingComments = false;
  isSubmittingComment = false;

  private route = inject(ActivatedRoute);
  private message = inject(NzMessageService);

  authService = inject(AuthService);
  hasPurchased = false;

  constructor(private fb: FormBuilder) {
    this.commentForm = this.fb.group({
      comment: ['', [
        Validators.maxLength(5000),
        (control) => {
          if (control.value && control.value.trim().length === 0) {
            return { whitespace: true };
          }
          return null;
        }
      ]],
    });
  }

  ngOnInit(): void {
    this.loadGuide()
  }

  private loadGuide(): void {
    const guideId = this.route.snapshot.paramMap.get('id');

    if (!guideId) {
      this.isGuideNotFound = true;
      console.log("No id in params")
      this.isGuideLoading = false;
      return;
    }

    this.guideService.getGuideById(guideId)
      .pipe(
        catchError(error => {
          this.isGuideNotFound = true;
          return of(null);
        }),
        finalize(() => {
          this.isGuideLoading = false;
        })
      )
      .subscribe({
        next: (guide) => {
          if (!guide) {
            this.isGuideNotFound = true;
            return;
          }

          this.guide = guide;
          this.loadLikeState(guideId);
          this.loadComments(guideId);
          this.checkPurchaseStatus(guideId);
          
          // Only initialize map if guide has places
          if (guide.places?.length) {
            this.initializeMap();
          }
        }
      });
  }

  private loadLikeState(guideId: string): void {
    // Check if current user has liked
    this.likeService.hasLiked(guideId, 'AudioGuide')
      .pipe(
        catchError((error) => {
          console.error('Error checking like status:', error);
          return of(false);
        })
      )
      .subscribe(hasLiked => {
        console.log('hasLiked', hasLiked);
        this.isLiked = hasLiked;
      });
  }

  initializeMap(): void {
    if (this.isMapInitialised || !this.guide?.places?.length) return;

    const checkContainer = () => {
      if (!this.mapContainer?.nativeElement) {
        console.log('Map container not yet available');
        // wait for 100ms if the map container has not been loaded
        setTimeout(checkContainer, 100);
        return;
      }

      this.map = new Map({
        container: this.mapContainer.nativeElement,
        style: `https://api.maptiler.com/maps/streets-v2/style.json?key=${environment.maptilerApiKey}`,
        center: this.initialCenter,
        zoom: 14,
        interactive: false,
      });

      this.isMapInitialised = true;

      this.addMarkersToMap();
    };

    checkContainer();
  }

  private clearMarkers(): void {
    this.markers.forEach(marker => marker.remove());
    this.markers = [];
  }

  private addMarkersToMap(): void {
    if (!this.map || !this.guide?.places?.length) return;

    this.clearMarkers();

    this.guide.places.forEach(place => {
      const coordinates = JSON.parse(place.coordinates);
      if (!coordinates?.coordinates || !Array.isArray(coordinates.coordinates)) return;

      const [longitude, latitude] = coordinates.coordinates;
      const marker = new Marker({ color: 'red' })
        .setLngLat([longitude, latitude])
        .addTo(this.map);

      this.markers.push(marker);
    });

    // Fit bounds to show all markers
    if (this.markers.length > 0) {
      const bounds = this.markers.reduce((bounds, marker) => {
        return bounds.extend(marker.getLngLat());
      }, new LngLatBounds());

      this.map.fitBounds(bounds, {
        padding: 50,
        maxZoom: 15
      });
    }
  }

  onLikeClick(): void {
    if (!this.guide) return;

    this.isLoadingLike = true;
    const guideId = this.guide.id;

    if (this.isLiked) {
      this.likeService.unlikeEntity(guideId, 'AudioGuide')
        .pipe(
          catchError((error) => {
            console.error('Error unliking guide:', error);
            this.message.error('Failed to unlike guide');
            return of(undefined);
          }),
          finalize(() => {
            this.isLoadingLike = false;
          })
        )
        .subscribe(() => {
          this.isLiked = false;
          if (this.guide) {
            this.guide.likeCount = Math.max(0, this.guide.likeCount - 1);
          }
        });
    } else {
      this.likeService.likeEntity(guideId, 'AudioGuide')
        .pipe(
          catchError((error) => {
            console.error('Error liking guide:', error);
            this.message.error('Failed to like guide');
            return of(undefined);
          }),
          finalize(() => {
            this.isLoadingLike = false;
          })
        )
        .subscribe(() => {
          this.isLiked = true;
          if (this.guide) {
            this.guide.likeCount++;
          }
        });
    }
  }

  private loadComments(guideId: string): void {
    this.isLoadingComments = true;
    this.commentService.getComments(guideId, 'AudioGuide')
      .pipe(
        catchError((error) => {
          console.error('Error loading comments:', error);
          this.message.error('Failed to load comments');
          return of([]);
        }),
        finalize(() => {
          this.isLoadingComments = false;
        })
      )
      .subscribe(comments => {
        this.comments = comments;
      });
  }

  submitComment(): void {
    if (this.commentForm.invalid || !this.guide) return;

    this.isSubmittingComment = true;
    const content = this.commentForm.value.comment;
    const guideId = this.guide.id;

    this.commentService.createComment(content, guideId, 'AudioGuide')
      .pipe(
        catchError((error) => {
          console.error('Error submitting comment:', error);
          this.message.error('Failed to submit comment');
          return of(undefined);
        }),
        finalize(() => {
          this.isSubmittingComment = false;
        })
      )
      .subscribe(comment => {
        if (comment) {
          // Reload all comments to ensure we have complete data
          this.loadComments(guideId);
          this.commentForm.reset();
          this.message.success('Comment posted successfully!');
        }
      });
  }

  formatCommentDate(date: string | null | undefined): string | undefined {
    if (!date) return undefined;
    return new DatePipe('en-US').transform(date, 'medium') || undefined;
  }

  private checkPurchaseStatus(guideId: string) {
    const userId = this.authService.getUserId();
    if (userId) {
      this.guideService.getGuidePurchaseStatus(userId, guideId).subscribe({
        next: (status) => {
          this.hasPurchased = status;
        },
        error: (err) => {
          console.error('Error checking purchase status:', err);
          this.hasPurchased = false;
        }
      });
    }
  }

  onPurchaseClick() {
    if (!this.guide) return;

    this.guideService.createCheckoutSession(this.guide.id).subscribe({
      next: (checkoutUrl) => {
        window.location.href = checkoutUrl;
      },
      error: (err) => {
        console.error('Failed to create checkout session:', err);
        this.message.error('Failed to initiate purchase');
      }
    });
  }
}
