import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GuideDto } from '../../shared/interfaces/guides/guide-dto';
import { GuidePostDto } from '../../shared/interfaces/guides/guide-post-dto';
import { GuideDetailsDto } from '../../shared/interfaces/guides/guide-details-dto';

@Injectable({
  providedIn: 'root',
})
export class GuideService {
  private apiUrl = `${environment.apiUrl}/AudioGuides`;
  constructor(private http: HttpClient) {}

  // POST: Create a new guides
  createGuide(guide: GuidePostDto): Observable<string> {
    console.log('Creating place');
    return this.http.post<string>(`${this.apiUrl}/create`, guide);
  }

  // GET: Get guide by ID
  getGuideById(id: string): Observable<GuideDetailsDto> {
    return this.http.get<GuideDetailsDto>(`${this.apiUrl}/${id}`);
  }

  // PUT: Update an existing guide
  updateGuide(id: string, guide: GuideDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, guide);
  }

  // DELETE: Delete a guide by ID
  deletePlace(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
