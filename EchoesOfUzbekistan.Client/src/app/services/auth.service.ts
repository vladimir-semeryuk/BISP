import { UserProfileService } from './users/user-profile.service';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { LoginRequest } from '../shared/interfaces/auth/login-request';
import { catchError, map, Observable, of, switchMap, tap } from 'rxjs';
import { LoginResponse } from '../shared/interfaces/auth/login-response';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl: string = environment.apiUrl;
  private accessToken = 'token';

  constructor(private http:HttpClient, private userProfileService:UserProfileService) { }


  login(req: LoginRequest): Observable<string | null> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/Users/login`, req, { observe: 'response' }).pipe(

      map(response => {
        if (response.status === 200 && response.body) {
          const token = response.body.accessToken;
          localStorage.setItem(this.accessToken, token);
          return token;
        }
        return null;
      }),
      
      switchMap(token => {
        if (token) {
          return this.userProfileService.refreshUserProfile().pipe(
            tap(profile => console.log('User profile loaded:', profile)),
            // Return the token after refreshing the profile.
            map(() => token)
          );
        }
        return of(null);
      }),
      catchError(error => {
        console.error('Login failed:', error);
        return of(null);
      })
    );
  }

  isLoggedIn = (): boolean => {
    const token = this.getToken()
    if (!token) return false
    return !this.isTokenExpired()
  }

  private isTokenExpired() {
    const token = this.getToken()
    if (!token) return true
    const decoded = jwtDecode(token)
    const isTokenExpired = Date.now() > decoded['exp']! * 1000 // TODO: change this logic later
    if (isTokenExpired) this.logout()
    return isTokenExpired    
  }

  logout() {
    localStorage.removeItem(this.accessToken)
    this.userProfileService.clearCache()
  }

  getToken = (): string | null => localStorage.getItem(this.accessToken) || '';
  
  getUserAuthDetail = () => {
    const token = this.getToken()
    if (!token) return null
    const decodedToken:any = jwtDecode(token)
    const userAuthDetails = {
      id: decodedToken.sub,
      fullName: decodedToken.name,
      email: decodedToken.email
    }
    return userAuthDetails
  }
}

