import { Component, Input } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { SelectLanguageDropdownComponent } from "../../../../../shared/components/select-language-dropdown/select-language-dropdown.component";

@Component({
  selector: 'app-create-place-modal-form',
  imports: [NzModalModule, ReactiveFormsModule, NzFormModule, SelectLanguageDropdownComponent],
  templateUrl: './create-place-modal-form.component.html',
  styleUrl: './create-place-modal-form.component.less',
})
export class CreatePlaceModalFormComponent {
  @Input() pointCoordinates: any;
  @Input() currentUserId!: string;

  placeForm!: FormGroup;
  isVisible = false;
  isOkLoading = false;

  showModal(): void {
    this.isVisible = true;
    this.placeForm.patchValue({
      authorId: this.currentUserId,
      pointCoordinates: this.pointCoordinates
    });
  }

  handleOk(): void {
    this.isOkLoading = true;
    setTimeout(() => {
      this.isVisible = false;
      this.isOkLoading = false;
    }, 3000);
  }

  handleCancel(): void {
    this.isVisible = false;
  }

  submitForm(): void {

  }
}
