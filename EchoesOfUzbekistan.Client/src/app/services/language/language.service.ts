// language.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, of, tap } from 'rxjs';
import { Language } from '../../shared/interfaces/common/Language';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  private languagesUrl = '/languages.json';

  constructor(private http: HttpClient) {}

  getLanguages(): Observable<Language[]> {
    return this.http.get<Language[]>(this.languagesUrl).pipe(
      tap({
        next: (languages) => {
          console.debug('Server response:', languages);
          
          // console.table(languages);  // Nice table format in browser console
          // languages.forEach(lang => {
          //   console.log(`Language: ${lang.code} - ${lang.name}`);
          // });
        },
        error: (err) => {
          console.error('Error fetching languages:', err);
          
          console.error('Error details:', {
            url: this.languagesUrl,
            status: err.status,
            message: err.message
          });
        },
        finalize: () => console.log('Language fetch operation completed')
      }),
      catchError(err => {
        return of([]);
      })
    );
  }
}
