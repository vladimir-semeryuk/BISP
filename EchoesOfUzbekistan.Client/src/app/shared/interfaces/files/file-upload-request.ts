import { EntityType } from "../common/entity-type";

export interface FileUploadRequest {
    fileName: string,
    contentType: string,
    entityType: EntityType
}
