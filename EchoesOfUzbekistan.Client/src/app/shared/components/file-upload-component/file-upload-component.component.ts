import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NzUploadModule, NzUploadXHRArgs, NzUploadFile } from 'ng-zorro-antd/upload';
import { FileService } from '../../../services/files/file-upload.service';
import { EntityType } from '../../interfaces/common/entity-type';
import { firstValueFrom, Subscription } from 'rxjs';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { UploadPresignedUrl } from '../../interfaces/files/presigned-url';
import { FileType } from '../../interfaces/files/file-type';
import { FileUploadResult } from '../../interfaces/files/file-upload-result';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-file-upload',
  imports: [NzUploadModule, NzButtonModule, NzIconModule],
  templateUrl: './file-upload-component.component.html',
  styleUrls: ['./file-upload-component.component.less']
})
export class FileUploadComponent {
  @Input() entityType!: EntityType;
  @Input() fileType!: FileType;
  @Output() fileUploaded = new EventEmitter<FileUploadResult>(); 
  fileList: NzUploadFile[] = [];

  constructor(private fileService: FileService, private message: NzMessageService) {}

  get acceptedFileTypes(): string {
    return this.fileType === FileType.IMAGE
      ? 'image/png,image/jpeg,image/gif,image/bmp'
      : 'audio/mpeg,audio/wav,audio/ogg';
  }

  beforeUpload = (file: NzUploadFile): boolean => {
    const isValid = this.acceptedFileTypes.split(',').includes(file.type || 'default-file-type');
    if (!isValid) {
      this.message.error(`Invalid file type. Please upload a ${this.fileType.toLowerCase()} file.`);
    }
    return isValid; // if set to false, file isn't uploaded
  };

  customUpload = (item: NzUploadXHRArgs): Subscription => {
    const { file, onSuccess, onError } = item;

    const fileFinal = file as unknown as File;
    const entityType: EntityType = this.entityType;
    const subscription = new Subscription();

    
    (async () => {
      try {
        if (!this.acceptedFileTypes.split(',').includes(fileFinal.type)) {
          this.message.error(`Invalid file type. Please upload a ${this.fileType.toLowerCase()} file.`);
          onError?.(new Error('Invalid file type'), file);
          return;
        }
        // Get presigned URLs from the backend
        const urls: UploadPresignedUrl = await this.fileService.getPresignedUrl(file.name, fileFinal.type, entityType);
        console.log('Presigned URLs:', urls);

        // Upload using the PUT URL
        console.log("Uploading to R2 with URL:", urls.putUrl);
        await firstValueFrom(this.fileService.uploadFileToR2(urls.putUrl, fileFinal));

        const fileKey = this.fileService.extractFileKey(urls.getUrl);
        console.log(`Uploaded file key is`, fileKey);
        this.fileUploaded.emit({ fileKey, fileType: this.fileType });

        // for NzUpload to set hyperlink.
        file.url = urls.getUrl;

        // Add the file to the fileList.
        this.fileList = [...this.fileList, file];

        onSuccess?.({ url: urls.getUrl }, file, null);
      } catch (error) {
        console.error('Upload failed:', error);
        onError?.(error, file);
      }
    })();

    return subscription;
  };
}
