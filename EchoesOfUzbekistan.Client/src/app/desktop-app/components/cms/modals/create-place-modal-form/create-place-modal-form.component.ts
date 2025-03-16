import { PlaceService } from './../../../../../services/place/place.service';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { SelectLanguageDropdownComponent } from '../../../../../shared/components/select-language-dropdown/select-language-dropdown.component';
import { PlacePost } from '../../../../../shared/interfaces/places/place-post';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzMessageService } from 'ng-zorro-antd/message';
import { PlaceNewlyCreatedDetails } from '../../../../../shared/interfaces/places/place-newly-created';

@Component({
  selector: 'app-create-place-modal-form',
  standalone: true,
  imports: [
    NzModalModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    SelectLanguageDropdownComponent,
  ],
  templateUrl: './create-place-modal-form.component.html',
  styleUrl: './create-place-modal-form.component.less',
})
export class CreatePlaceModalFormComponent {
  // @Input() pointCoordinates!: [number, number];
  @Input() currentUserId!: string;
  @Output() placeSaved = new EventEmitter<PlaceNewlyCreatedDetails>();
  @Output() modalClosed = new EventEmitter<void>();

  placeForm!: FormGroup;
  isVisible = false;
  isOkLoading = false;

  constructor(
    private fb: FormBuilder,
    private placeService: PlaceService,
    private message: NzMessageService
  ) {}

  ngOnInit() {
    this.placeForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.maxLength(5000)],
      authorId: [this.currentUserId],
      longitude: 0.0,
      latitude: 0.0,
      audioLink: [''],
      imageLink: [''],
    });
  }

  showModal(lon: number, lat: number): void {
    console.log('MODAL VISIBLE');
    // the timeout is set to prevent accidental misclicks that cause the modal to be closed
    setTimeout(() => {
      this.isVisible = true;
    }, 50);
    this.placeForm.patchValue({
      authorId: this.currentUserId,
      longitude: lon,
      latitude: lat,
    });
  }

  handleCancel(): void {
    if (!this.isVisible) return;
    this.isVisible = false;
    this.modalClosed.emit();
    console.log('Modal closed handleCancel');
  }

  submitForm(): void {
    if (this.placeForm.invalid) {
      console.log('Form is invalid');
      console.log(this.placeForm.value);
      return;
    }

    const placeData: PlacePost = this.placeForm.value;

    this.isOkLoading = true;
    const loadingMessageId = this.message.loading('Creating place...', {
      nzDuration: 0,
    }).messageId;

    console.log('Submit form works');
    console.log(placeData);
    this.placeService.createPlace(placeData).subscribe({
      next: (res) => {
        this.isOkLoading = false;
        this.isVisible = false;
        console.log('Modal closed submitForm');

        this.message.remove(loadingMessageId);

        // Display success message
        this.message.success('Place created successfully!');

        const createdPlace: PlaceNewlyCreatedDetails = {
          id: res,
          title: placeData.title,
          description: placeData.description,
        };
        this.placeSaved.emit(createdPlace);
        this.placeForm.reset();
      },
      error: (err) => {
        console.error('Error creating place:', err);

        // Remove loading message
        this.message.remove(loadingMessageId);

        // Display error message
        this.message.error('Error creating place.');

        this.modalClosed.emit();
        this.isOkLoading = false;
      },
    });
    // this.placeService.createPlace()
  }
}
