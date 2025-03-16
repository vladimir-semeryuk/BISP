import { Routes } from '@angular/router';
import { DHomeScreenComponent } from './desktop-app/components/screens/d-home-screen/d-home-screen.component';
import { DLoginScreenComponent } from './desktop-app/components/screens/d-login-screen/d-login-screen.component';
import { DSignupScreenComponent } from './desktop-app/components/screens/d-signup-screen/d-signup-screen.component';
import { AuthGuard } from './shared/guards/auth.guard';
import { OwnContentGuard } from './shared/guards/own-content.guard';
import { DProfileScreenComponent } from './desktop-app/components/screens/d-profile-screen/d-profile-screen.component';
import { ExampleFormComponent } from './example-form-component/example-form-component.component';
import { DModifyGuideScreenComponent } from './desktop-app/components/screens/cms/d-modify-guide-screen/d-modify-guide-screen.component';

export const routes: Routes = [
  { path: '', component: DHomeScreenComponent},
  { path: 'login', component: DLoginScreenComponent},
  { path: 'signup', component: DSignupScreenComponent},
  {
    path: 'profile',
    component: DProfileScreenComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'text', component: ExampleFormComponent
  },
  {
    path: 'modify-guide', component: DModifyGuideScreenComponent,
    canActivate: [AuthGuard]
  }
];
