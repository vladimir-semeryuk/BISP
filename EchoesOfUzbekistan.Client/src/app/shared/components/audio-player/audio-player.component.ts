import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GuideService } from '../../../services/guides/guide.service';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-audio-player',
  standalone: true,
  imports: [CommonModule, NzButtonModule, NzIconModule],
  template: `
    <div class="audio-player">
      <audio #audioPlayer controls [src]="streamUrl" (error)="onError()">
        Your browser does not support the audio element.
      </audio>
      <div class="controls">
        <button nz-button nzType="primary" (click)="togglePlay()">
          <i nz-icon [nzType]="isPlaying ? 'pause' : 'play'"></i>
          {{ isPlaying ? 'Pause' : 'Play' }}
        </button>
      </div>
    </div>
  `,
  styles: [`
    .audio-player {
      display: flex;
      flex-direction: column;
      gap: 10px;
      padding: 10px;
      background: #f5f5f5;
      border-radius: 4px;
    }
    .controls {
      display: flex;
      gap: 10px;
      justify-content: center;
    }
  `]
})
export class AudioPlayerComponent implements OnInit {
  @Input() guideId!: string;
  @Input() hasPurchased: boolean = false;

  streamUrl: string = '';
  isPlaying: boolean = false;
  private audioElement?: HTMLAudioElement;

  constructor(
    private guideService: GuideService,
    private message: NzMessageService
  ) {}

  ngOnInit() {
    if (this.hasPurchased) {
      this.streamUrl = `${this.guideService.apiUrl}/stream/${this.guideId}`;
    }
  }

  togglePlay() {
    if (!this.audioElement) {
      const element = document.querySelector('audio');
      if (element) {
        this.audioElement = element;
      }
    }
    if (this.audioElement) {
      if (this.isPlaying) {
        this.audioElement.pause();
      } else {
        this.audioElement.play();
      }
      this.isPlaying = !this.isPlaying;
    }
  }

  onError() {
    this.message.error('Failed to load audio. Please try again later.');
  }
} 