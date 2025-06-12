export interface CreateCommentDto {
    content: string;
    entityId: string;
    entityType: string;
  }
  
  export interface CommentDto {
    id: string;
    content: string;
    authorId: string;
    authorName: string;
    authorAvatar: string;
    entityId: string;
    entityType: string;
    dateCreated: string;
    likes: number;
  } 