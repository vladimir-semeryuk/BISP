import { FileUploadRequest } from './../../shared/interfaces/files/file-upload-request';
import { EntityType } from './../../shared/interfaces/common/entity-type';
// upload.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment.development';
import { UploadPresignedUrl } from '../../shared/interfaces/files/presigned-url';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  private apiBaseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getPresignedUrls(inputParams: FileUploadRequest): Observable<UploadPresignedUrl> {
    const params = new HttpParams()
    .set('fileName', encodeURIComponent(inputParams.fileName))
    .set('contentType', inputParams.contentType)
    .set('entityType', inputParams.entityType);

    return this.http.post<UploadPresignedUrl>(
      `${this.apiBaseUrl}/Files/upload`,
      null,  // Request body (empty for file upload via query params)
      { params }
    ).pipe(
      catchError(error => {
        console.error('Upload error:', error);
        return throwError(() => new Error('File upload failed. Please try again.'));
      })
    );
  }

  uploadFileToS3(presignedData: UploadPresignedUrl, file: File): Observable<string> {
    return this.http.put(presignedData.put_url, file, {
      headers: new HttpHeaders({
        'Content-Type': file.type,
        'x-amz-meta-file-name': file.name
      }),
      observe: 'response'
    }).pipe(
      map((response: HttpResponse<any>) => {
        if (response.status >= 200 && response.status < 300) {
          return presignedData.get_url;
        } else {
          throw new Error(`Upload failed with status ${response.status}`);
        }
      }),
      catchError(err => {
        console.error('Upload error:', err);
        return throwError(() => new Error('File upload failed. Please try again.'));
      })
    );
  }

  uploadFile(file: File, entityType: EntityType): Observable<string> {
    const inputParams: FileUploadRequest = {fileName: file.name, contentType: file.type, entityType:entityType}
    return this.getPresignedUrls(inputParams).pipe(
      switchMap((presignedData: UploadPresignedUrl) => this.uploadFileToS3(presignedData, file))
    );
  }
}
