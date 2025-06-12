import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { LikedGuideDto } from '../../shared/interfaces/guides/liked-guide-dto';

@Injectable({
  providedIn: 'root',
})
export class LikeService {
  private apiUrl = `${environment.apiUrl}/Likes`;

  constructor(private http: HttpClient) {}

  // POST: Like an entity
  likeEntity(entityId: string, entityType: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}`, {
      entityId,
      entityType
    });
  }

  // DELETE: Unlike an entity
  unlikeEntity(entityId: string, entityType: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/unlike`, {
      body: {
        entityId,
        entityType
      }
    });
  }

  // GET: Get like count for an entity
  getLikeCount(entityId: string, entityType: string): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/count`, {
      params: {
        entityId,
        entityType
      }
    });
  }

  // GET: Check if current user has liked an entity
  hasLiked(entityId: string, entityType: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/has-liked`, {
      params: {
        entityId,
        entityType
      }
    });
  }

  // GET: Gets all the liked guides for a user
  getLikedGuides(userId: string, pageNumber: number, pageSize: number): Observable<LikedGuideDto[]> {
    return this.http.get<LikedGuideDto[]>(
      `${this.apiUrl}/liked`,
      {
        params: { 
          userId,
          pageNumber: pageNumber.toString(),
          pageSize: pageSize.toString()
        }
      }
    ).pipe(
      catchError(error => {
        console.error('Error fetching liked guides:', error);
        return of([]);
      })
    );
  }
} 
