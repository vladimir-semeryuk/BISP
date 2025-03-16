import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { environment } from '../../environments/environment.development';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const apiBaseUrl = environment.apiUrl; 

  if (req.url.startsWith(apiBaseUrl)) {
    const token = authService.getToken();
    if (token) {
      const cloned = req.clone({
        headers: req.headers.set('Authorization', 'Bearer ' + token)
      });
      return next(cloned);
    }
  }
  return next(req);
};
