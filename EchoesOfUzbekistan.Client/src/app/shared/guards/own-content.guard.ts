import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, GuardResult, MaybeAsync, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class OwnContentGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean | UrlTree {
    const contentOwnerId = route.data['ownerId'];
    const currentUser = this.authService.getUserAuthDetail();

    if (currentUser === null){
      return this.router.parseUrl('/unauthorized')
    }

    const currentUserId = currentUser.id

    if (currentUserId && contentOwnerId && currentUserId === contentOwnerId) {
      return true;
    }

    return this.router.parseUrl('/unauthorized');
  }  
}
