import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { environment } from '../../environments/environment.development';
import { switchMap, throwError, catchError } from 'rxjs';


// export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
//   const authService = inject(AuthService);
//   const apiBaseUrl = environment.apiUrl; 

//   if (req.url.startsWith(apiBaseUrl)) {
//     const token = authService.getToken();
//     if (token) {
//       const cloned = req.clone({
//         headers: req.headers.set('Authorization', 'Bearer ' + token)
//       });
//       return next(cloned).pipe(
//         catchError((err: HttpErrorResponse) => {
//           if (err.status === 401) {
//             return authService.refreshToken().pipe(
//               switchMap(newToken => {
//                 if (newToken) {
//                   const retryRequest = req.clone({
//                     headers: req.headers.set('Authorization', 'Bearer ' + newToken)
//                   });
//                   return next(retryRequest);
//                 }
//                 return throwError(() => err); // fail if no token
//               }),
//               catchError(refreshError => {
//                 authService.logout()
//                 return throwError(() => refreshError);
//               })
//             );
//           }
//           return throwError(() => err);
//         })
//       );
//     }
//   }

//   return next(req);
// };

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const apiBaseUrl = environment.apiUrl;

  // Only intercept API requests
  if (req.url.startsWith(apiBaseUrl)) {
    // Clone the request to include withCredentials: true to ensure cookies are sent with the request
    const clonedReq = req.clone({
      withCredentials: true
    });

    return next(clonedReq).pipe(
      catchError((err: HttpErrorResponse) => {
        // Only handle 401 errors for the refresh token endpoint
        if (err.status === 401 && !req.url.includes('/Users/refresh-token')) {
          return authService.refreshToken().pipe(
            switchMap(newToken => {
              if (newToken) {
                // Retry the original request after successful refresh
                const clonedRetryReq = clonedReq.clone({
                  withCredentials: true
                });
                return next(clonedRetryReq);
              }
              // If refresh token failed, don't retry the request
              return throwError(() => err);
            }),
            catchError(refreshError => {
              // Only logout if the refresh token request itself failed
              if (refreshError.status === 401) {
                authService.logout();
              }
              return throwError(() => refreshError);
            })
          );
        }
        return throwError(() => err);
      })
    );
  }

  return next(req);
};
