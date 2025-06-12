import {
  ApplicationConfig,
  provideZoneChangeDetection,
  importProvidersFrom,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { en_US, provideNzI18n } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { FormsModule } from '@angular/forms';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideNzIcons } from 'ng-zorro-antd/icon';
import { IconDefinition } from '@ant-design/icons-angular';
import {
  MenuOutline,
  SearchOutline,
  LockOutline,
  UserOutline,
  MessageOutline,
  QuestionCircleOutline,
  SettingOutline,
  EditOutline,
  MoreOutline,
  CloseOutline,
  DownloadOutline,
  CloudDownloadOutline,
  FileTwoTone,
  LikeOutline,
  PlaySquareOutline,
  WarningOutline,
  LikeFill
} from '@ant-design/icons-angular/icons';
import { tokenInterceptor } from './interceptors/token.interceptor';

const DownloadOOutline: IconDefinition = {
  ...DownloadOutline,
  name: 'download-o', // Override the name to fix an error with FileUpload component
};

const icons: IconDefinition[] = [
  MenuOutline,
  SearchOutline,
  LockOutline,
  UserOutline,
  MessageOutline,
  QuestionCircleOutline,
  EditOutline,
  MoreOutline,
  SettingOutline,
  CloseOutline,
  DownloadOutline,
  DownloadOOutline,
  CloudDownloadOutline,
  FileTwoTone,
  LikeOutline,
  PlaySquareOutline,
  WarningOutline,
  LikeFill
];

registerLocaleData(en);

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideNzIcons(icons),
    provideRouter(routes),
    provideNzI18n(en_US),
    importProvidersFrom(FormsModule),
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([tokenInterceptor])),
  ],
};
