import { Injectable } from '@angular/core';
import { UserDetails } from '../../shared/interfaces/users/UserDetails';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, catchError, of } from 'rxjs';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {
  private storageKey = 'userDetails';
  // Using BehaviorSubject to hold the current user profile
  private userProfileSubject = new BehaviorSubject<UserDetails | null>(null);
  public userProfile$ = this.userProfileSubject.asObservable();

  constructor(private http: HttpClient) {
    // Get cached details from sessionStorage if available
    const stored = sessionStorage.getItem(this.storageKey);
    if (stored) {
      try {
        const profile: UserDetails = JSON.parse(stored);
        this.userProfileSubject.next(profile);
      } catch (error) {
        console.error('Error parsing stored user profile:', error);
      }
    }
  }

  getUserProfile(): Observable<UserDetails | null> {
    return this.userProfile$;
  }

  refreshUserProfile(): Observable<UserDetails> {
    return this.http.get<UserDetails>(`${environment.apiUrl}/Users/me`).pipe(
      tap(profile => {
        this.userProfileSubject.next(profile);
        sessionStorage.setItem(this.storageKey, JSON.stringify(profile));
      }),
      catchError(error => {
        console.error('Error refreshing user profile:', error);
        return of(null as any);
      })
    );
  }

  clearCache(): void {
    sessionStorage.removeItem(this.storageKey);
    this.userProfileSubject.next(null);
  }

  getUserProfileById(userId: string): Observable<UserDetails> {
    return this.http.get<UserDetails>(`${environment.apiUrl}/Users/${userId}`).pipe(
      catchError(error => {
        console.error(`Error fetching profile for user ${userId}:`, error);
        return of(null as any);
      })
    );
  }
}
