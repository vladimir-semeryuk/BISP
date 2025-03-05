import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap, catchError, of } from 'rxjs';
import { Country } from '../../shared/interfaces/common/country';

@Injectable({
  providedIn: 'root'
})
export class CountryService {
  private countriesUrl = '/countries.json';

  constructor(private http: HttpClient) {}

  getCountries(): Observable<Country[]> {
    return this.http.get<Country[]>(this.countriesUrl).pipe(
      tap({
        next: (languages) => {
          console.debug('Server response:', languages);
        },
        error: (err) => {
          console.error('Error fetching countries:', err);

          console.error('Error details:', {
            url: this.countriesUrl,
            status: err.status,
            message: err.message
          });
        },
        finalize: () => console.log('Country fetch operation completed')
      }),
      catchError(err => {

        return of([]);
      })
    );
  }
}
