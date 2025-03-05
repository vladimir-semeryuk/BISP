import { EntityType } from './../shared/interfaces/common/entity-type';
// example-form.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { SelectLanguageDropdownComponent } from "../shared/components/select-language-dropdown/select-language-dropdown.component";
import { NzFormModule } from 'ng-zorro-antd/form';
import { FileUploadComponentComponent } from "../shared/components/file-upload-component/file-upload-component.component";

@Component({
  selector: 'app-example-form',
  template: `
    <form nzForm [formGroup]="form" (ngSubmit)="onSubmit()">
      <nz-form-item>
      <nz-form-control [nzSpan]="12" nzErrorTip="Please select language!">
        <app-select-language-dropdown controlTitle="language" id="language" [required]="true"></app-select-language-dropdown>
      </nz-form-control>
      </nz-form-item>
      <app-file-upload-component [entityType]="entityType"></app-file-upload-component>
      <button nz-button nzType="primary" type="submit">Submit</button>
    </form>
  `,
  imports: [SelectLanguageDropdownComponent, NzFormModule, ReactiveFormsModule, FileUploadComponentComponent]
})
export class ExampleFormComponent implements OnInit {
  form!: FormGroup;
  entityType: EntityType = EntityType.Guide;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      language: ['en'] 
    });
  }

  onSubmit(): void {
    console.log('Selected language:', this.form.value.language);
  }
}
