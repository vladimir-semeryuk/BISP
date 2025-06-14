import { Routes } from '@angular/router';
import { DHomeScreenComponent } from './desktop-app/components/screens/d-home-screen/d-home-screen.component';
import { DLoginScreenComponent } from './desktop-app/components/screens/d-login-screen/d-login-screen.component';
import { DSignupScreenComponent } from './desktop-app/components/screens/d-signup-screen/d-signup-screen.component';
import { AuthGuard } from './shared/guards/auth.guard';
import { OwnContentGuard } from './shared/guards/own-content.guard';
import { DProfileScreenComponent } from './desktop-app/components/screens/d-profile-screen/d-profile-screen.component';
import { ExampleFormComponent } from './example-form-component/example-form-component.component';
import { DModifyGuideScreenComponent } from './desktop-app/components/screens/cms/d-modify-guide-screen/d-modify-guide-screen.component';
import { DCmsHomeScreenComponent } from './desktop-app/components/screens/cms/d-cms-home-screen/d-cms-home-screen.component';
import { ForgotPasswordScreenComponent } from './desktop-app/components/screens/forgot-password-screen/forgot-password-screen.component';
import { DViewSingleGuideScreenComponent } from './desktop-app/components/screens/d-view-single-guide-screen/d-view-single-guide-screen.component';

export const routes: Routes = [
  { path: '', component: DHomeScreenComponent },
  { path: 'login', component: DLoginScreenComponent },
  { path: 'signup', component: DSignupScreenComponent },
  {
    path: 'profile',
    component: DProfileScreenComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'profile/:id',
    component: DProfileScreenComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'text',
    component: ExampleFormComponent,
  },
  {
    path: 'cms/my-guides',
    component: DCmsHomeScreenComponent,
  },
  {
    path: 'modify-guide',
    component: DModifyGuideScreenComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordScreenComponent,
  },
  {
    path: 'view-guide/:id',
    component: DViewSingleGuideScreenComponent,
  },
];
