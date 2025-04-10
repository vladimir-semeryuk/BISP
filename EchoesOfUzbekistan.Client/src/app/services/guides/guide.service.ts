import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GuideDto } from '../../shared/interfaces/guides/guide-dto';
import { GuidePostDto } from '../../shared/interfaces/guides/guide-post-dto';
import { GuideDetailsDto } from '../../shared/interfaces/guides/guide-details-dto';
import { AudioGuideFilter } from '../../shared/interfaces/guides/audio-guide-filter';

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

  getGuides(filter: AudioGuideFilter): Observable<GuideDto[]> {
    let params = new HttpParams();

    if (filter.createdByUserId) {
      params = params.set('CreatedByUserId', filter.createdByUserId);
    }
    if (filter.getNewest !== undefined) {
      params = params.set('GetNewest', filter.getNewest.toString());
    }
    if (filter.getTopN !== undefined) {
      params = params.set('GetTopN', filter.getTopN.toString());
    }

    return this.http.get<GuideDto[]>(`${this.apiUrl}`, { params });
  }
}
