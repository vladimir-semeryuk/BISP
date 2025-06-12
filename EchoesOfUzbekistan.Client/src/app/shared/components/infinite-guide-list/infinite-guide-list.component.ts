import { Component, OnInit, Input, inject, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { CdkVirtualScrollViewport, CdkFixedSizeVirtualScroll, CdkVirtualForOf } from '@angular/cdk/scrolling';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LikeService } from '../../../services/likes/like.service';
import { LikedGuideDto } from '../../interfaces/guides/liked-guide-dto';

@Component({
  selector: 'app-infinite-guide-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    NzListModule,
    NzEmptyModule,
    NzSpinModule,
    CdkVirtualScrollViewport,
    CdkFixedSizeVirtualScroll,
    CdkVirtualForOf
  ],
  templateUrl: './infinite-guide-list.component.html',
  styleUrl: './infinite-guide-list.component.less',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InfiniteGuideListComponent implements OnInit {
  @Input() userId!: string;
  private likeService = inject(LikeService);
  private cdr = inject(ChangeDetectorRef);

  guides: LikedGuideDto[] = [];
  isLoading = true;
  hasNoGuides = false;

  ngOnInit(): void {
    this.likeService.getLikedGuides(this.userId, 1, 10)
      .pipe(
        catchError(error => {
          console.error('Error fetching guides:', error);
          return of([]);
        })
      )
      .subscribe(guides => {
        this.isLoading = false;
        this.hasNoGuides = guides.length === 0;
        this.guides = guides;
        this.cdr.detectChanges();
      });
  }
} 
