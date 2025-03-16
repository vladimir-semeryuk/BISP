import { EntityType } from './../shared/interfaces/common/entity-type';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { SelectLanguageDropdownComponent } from "../shared/components/select-language-dropdown/select-language-dropdown.component";
import { NzFormModule } from 'ng-zorro-antd/form';
import { FileUploadComponent } from "../shared/components/file-upload-component/file-upload-component.component";
import { FileService } from '../services/files/file-upload.service';
import { NzIconModule } from 'ng-zorro-antd/icon';

// THIS COMPONENT IS FOR TESTING PURPOSES ONLY
@Component({
  selector: 'app-example-form',
  template: `
    <form nzForm [formGroup]="form" (ngSubmit)="onSubmit()">
      <nz-form-item>
      <nz-form-control [nzSpan]="12" nzErrorTip="Please select language!">
        <app-select-language-dropdown controlTitle="language" id="language" [required]="true"></app-select-language-dropdown>
      </nz-form-control>
      </nz-form-item>
      <app-file-upload [entityType]="entityType"></app-file-upload>
      <button nz-button nzType="primary" type="submit">Submit</button>
    </form>
  `,
  imports: [SelectLanguageDropdownComponent, NzFormModule, ReactiveFormsModule, FileUploadComponent, NzIconModule]
})
export class ExampleFormComponent implements OnInit {
  form!: FormGroup;
  entityType: EntityType = EntityType.Guide;

  constructor(private fb: FormBuilder, private fileService: FileService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      language: ['en'] 
    });
    this.entityType = EntityType.Guide;
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;
  
    const file = input.files[0];
    this.fileService.uploadFile(file, EntityType.Guide).subscribe({
      next: (url) => console.log("Success, file at:", url),
      error: (err) => console.error("Upload error:", err)
    });
  }

  onSubmit(): void {
    console.log('Selected language:', this.form.value.language);
  }
}
