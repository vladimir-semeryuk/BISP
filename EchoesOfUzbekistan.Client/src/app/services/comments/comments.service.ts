import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CommentDto, CreateCommentDto } from '../../shared/interfaces/comments/comment-dto';

@Injectable({
  providedIn: 'root',
})
export class CommentService {
  private apiUrl = `${environment.apiUrl}/Comments`;

  constructor(private http: HttpClient) {}

  // POST: Create a new comment
  createComment(content: string, entityId: string, entityType: string): Observable<CommentDto> {
    const createCommentDto: CreateCommentDto = {
      content,
      entityId,
      entityType
    };
    return this.http.post<CommentDto>(`${this.apiUrl}`, createCommentDto, {
      headers: {
        'Content-Type': 'application/json'
      }
    });
  }

  // GET: Get comments for an entity
  getComments(entityId: string, entityType: string): Observable<CommentDto[]> {
    return this.http.get<CommentDto[]>(`${this.apiUrl}`, {
      params: {
        entityId,
        entityType
      }
    });
  }
} 
