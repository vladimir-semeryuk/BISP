import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PlacePost } from '../../shared/interfaces/places/place-post';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { PlaceDto } from '../../shared/interfaces/places/place-dto';

@Injectable({
  providedIn: 'root'
})
export class PlaceService {
  private apiUrl = `${environment.apiUrl}/Places`; 

  constructor(private http: HttpClient) { }

  // POST: Create a new place
  createPlace(place: PlacePost): Observable<string> {
    console.log("Creating place")
    return this.http.post<string>(`${this.apiUrl}/create`, place);
  }

  // GET: Get place by ID
  getPlaceById(id: string): Observable<PlaceDto> {
    return this.http.get<PlaceDto>(`${this.apiUrl}/${id}`);
  }

  // PUT: Update an existing place
  updatePlace(id: string, place: PlaceDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, place);
  }

  // DELETE: Delete a place by ID
  deletePlace(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
