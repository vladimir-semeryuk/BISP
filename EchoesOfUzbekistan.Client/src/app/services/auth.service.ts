import { UserProfileService } from './users/user-profile.service';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { LoginRequest } from '../shared/interfaces/auth/login-request';
import { BehaviorSubject, catchError, map, Observable, of, tap } from 'rxjs';
import { LoginResponse } from '../shared/interfaces/auth/login-response';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { ChangePasswordRequest } from '../shared/interfaces/auth/change-password-request';
import { UserDetails } from '../shared/interfaces/users/UserDetails';
import { Router } from '@angular/router';
import { TokenResponse } from '../shared/interfaces/auth/token-response';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl: string = environment.apiUrl;
  private router = inject(Router);
  private isRefreshing = false;
  private http = inject(HttpClient);
  private userProfileService = inject(UserProfileService);
  private cookieService = inject(CookieService);

  private userSubject = new BehaviorSubject<UserDetails | null>(null);
  user$ = this.userSubject.asObservable();

  private currentUserSubject = new BehaviorSubject<TokenResponse | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private httpClient: HttpClient) {
    // Load user from localStorage on initialization
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      this.currentUserSubject.next(JSON.parse(storedUser));
    }
  }

  loadUser() {
    return this.http
      .get<UserDetails>(`${environment.apiUrl}/Users/me`, {
        withCredentials: true,
      })
      .pipe(
        tap((user) => this.userSubject.next(user)),
        catchError(() => {
          this.userSubject.next(null);
          return of(null);
        })
      );
  }

  isLoggedIn(): boolean {
    console.log(`isLoggedIn returns: ${this.userSubject.value != null}`)
    return this.userSubject.value != null;
  }

  login(req: LoginRequest): Observable<boolean> {
    return this.http
      .post<LoginResponse>(`${this.apiUrl}/Users/login`, req, {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          if (response.status === 200 && response.body) {
            this.loadUser().subscribe();
            return true;
          }

          return false;
        }),
        catchError(() => {
          this.userSubject.next(null);
          return of(false);
        })
      );
  }

  refreshToken(): Observable<boolean> {
    if (this.isRefreshing) {
      return of(false); // prevent duplicate calls
    }
    this.isRefreshing = true;

    return this.http
      .post(
        `${this.apiUrl}/Users/refresh-token`,
        {},
        { observe: 'response', withCredentials: true }
      )
      .pipe(
        map((response) => {
          this.isRefreshing = false;
          if (response.status === 200) {
            this.loadUser().subscribe();
            return true;
          }
          return false;
        }),
        catchError((error: HttpErrorResponse) => {
          console.error('Refresh token failed:', error);
          this.isRefreshing = false;
          if (error.status === 401 || error.status === 403) {
            console.error(
              'Refresh token failed, user is unauthorized. Status:',
              error.status
            );
            this.logout();
          }
          return of(false);
        })
      );
  }

  ensureLoggedIn(): Observable<boolean> {
    if (this.isLoggedIn()) {
      console.log('Im logged in, 74 ensure logged in');
      return of(true); // If the user is already logged in based on cookies
    }

    return this.refreshToken().pipe(
      map((isLoggedIn) => {
        console.log('im logged in, lol. refreshing tokens rules');
        return isLoggedIn;
      }),
      catchError(() => {
        console.log('Failed to refresh token, user is not logged in');
        return of(false);
      })
    );
  }

  logout(): void {
    if (this.userSubject.value === null) {
      return;
    }

    this.http
      .post(`${this.apiUrl}/Users/logout`, {}, { withCredentials: true })
      .subscribe({
        next: () => {
          console.log('Logout request successful');
        },
        error: (err) => {
          console.error('Logout request failed:', err);
        },
        complete: () => {
          this.cookieService.delete('access_token');
          this.cookieService.delete('refresh_token');
          this.userProfileService.clearCache();
          this.userSubject.next(null);
          this.router.navigateByUrl('');
        },
      });
  }

  changePassword(
    changePasswordRequest: ChangePasswordRequest
  ): Observable<any> {
    return this.http.put(
      `${this.apiUrl}/Users/change-password`,
      changePasswordRequest
    );
  }

  getUserAuthDetail(): any {
    return this.http
      .get<UserDetails>(`${environment.apiUrl}/Users/me`, {
        withCredentials: true,
      })
      .pipe(
        tap((user) => this.userSubject.next(user)),
        catchError(() => {
          this.userSubject.next(null);
          return of(null);
        })
      );
  }

  getUserId(): string | null {
    const user = this.currentUserSubject.value;
    return user?.userId || null;
  }
}
