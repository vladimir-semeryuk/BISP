import { EntityType } from './../../shared/interfaces/common/entity-type';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { firstValueFrom, from, Observable, throwError } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment.development';
import { UploadPresignedUrl } from '../../shared/interfaces/files/presigned-url';


@Injectable({
  providedIn: 'root'
})
export class FileService {
  private apiBaseUrl = environment.apiUrl;
  private uploadEndpoint = `${this.apiBaseUrl}/Files/upload`;

  constructor(private http: HttpClient) {}

  async getPresignedUrl(fileName: string, contentType: string, entityType: EntityType): Promise<UploadPresignedUrl> {
    const type = entityType.toString();
    fileName = encodeURIComponent(fileName)

    const params = new URLSearchParams({ fileName, contentType, type });
    
    const response = await firstValueFrom(
      this.http.post<UploadPresignedUrl>(
        `${this.uploadEndpoint}?${params.toString()}`,
        {}
      )
    );

    return response;
  }

  uploadFileToR2(url: string, file: File): Observable<HttpResponse<any>> {
    const fileName = encodeURIComponent(file.name) // needed to support some symbols and letters

    return this.http.put(url, file, {
      headers: new HttpHeaders({
        'Content-Type': file.type,
        'x-amz-meta-file-name': fileName
      }),
      observe: 'response'
    }).pipe(
      tap((response: HttpResponse<any>) => {
        console.log("Upload response status:", response.status);
      }),
      catchError(err => {
        console.error("Upload error:", err);
        return throwError(() => new Error("File upload failed."));
      })
    );
  }

  uploadFile(file: File, entityType: EntityType): Observable<string> {
    return from(this.getPresignedUrl(file.name, file.type, entityType)).pipe(
      switchMap((urls: UploadPresignedUrl) => {
        if (!urls.putUrl || !urls.getUrl) {
          return throwError(() => new Error("Pre-signed URLs are missing."));
        }
        return this.uploadFileToR2(urls.putUrl, file).pipe(
          map(() => urls.getUrl)
        );
      })
    );
  }

  extractFileKey(presignedUrl: string): string {
    try {
      const urlObj = new URL(presignedUrl);
      return urlObj.pathname.substring(1);
    } catch (error) {
      console.error("Failed to extract key:", error);
      return "";
    }
  }
}
