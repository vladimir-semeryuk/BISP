import { Component, Input } from '@angular/core';
import { NzUploadModule, NzUploadXHRArgs } from 'ng-zorro-antd/upload';
import { FileUploadService } from '../../../services/files/file-upload.service';
import { EntityType } from '../../interfaces/common/entity-type';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-file-upload-component',
  imports: [NzUploadModule],
  templateUrl: './file-upload-component.component.html',
  styleUrl: './file-upload-component.component.less',
})
export class FileUploadComponentComponent {
  @Input({ required: true }) entityType!: EntityType;

  constructor(private uploadService: FileUploadService) {}

  // Custom request function as required by nz-upload.
  customRequest = (item: NzUploadXHRArgs): Subscription => {
    // item.file is the File object
    const file: File = item.file.originFileObj as File;

    return this.uploadService.uploadFile(file, this.entityType).subscribe({
      next: (resourceUrl: string) => {
        item.onSuccess!(resourceUrl, item.file, null);
      },
      error: (err) => {
        item.onError!(err, item.file);
      }
    });
  };
}
