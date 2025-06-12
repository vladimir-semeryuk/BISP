import { PriceInputComponent } from './../../../../../shared/components/price-input/price-input.component';
import { SelectLanguageDropdownComponent } from './../../../../../shared/components/select-language-dropdown/select-language-dropdown.component';
import { Component, inject, ViewChild } from '@angular/core';
import { NavbarComponent, NavbarMode } from '../../../../../shared/components/navbar/navbar.component';
import { AuthService } from '../../../../../services/auth.service';
import {
  NavLink,
  getCmsNavLinks,
} from '../../../../../shared/interfaces/NavLink';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { EditMapComponent } from '../../../cms/edit-map/edit-map.component';
import { FileUploadComponent } from '../../../../../shared/components/file-upload-component/file-upload-component.component';
import { EntityType } from '../../../../../shared/interfaces/common/entity-type';
import { FileUploadResult } from '../../../../../shared/interfaces/files/file-upload-result';
import { FileType } from '../../../../../shared/interfaces/files/file-type';
import { GuideService } from '../../../../../services/guides/guide.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { UserProfileService } from '../../../../../services/users/user-profile.service';

@Component({
  selector: 'app-d-modify-guide-screen',
  imports: [
    RouterModule,
    NzIconModule,
    NzDividerModule,
    NavbarComponent,
    ReactiveFormsModule,
    NzButtonModule,
    NzCheckboxModule,
    NzFormModule,
    NzInputModule,
    SelectLanguageDropdownComponent,
    NzToolTipModule,
    PriceInputComponent,
    EditMapComponent,
    FileUploadComponent,
  ],
  templateUrl: './d-modify-guide-screen.component.html',
  styleUrl: './d-modify-guide-screen.component.less',
})
export class DModifyGuideScreenComponent {
  
  authService = inject(AuthService);
  currentUserId: string = '';
  
  FileType = FileType;
  entityType: EntityType = EntityType.Guide;
  guideForm!: FormGroup;
  
  navLinks: NavLink[] | null = [];
  navbarMode = NavbarMode.CMS;
  isOkLoading = false;

  @ViewChild(EditMapComponent) editMapComponent!: EditMapComponent;

  constructor(
    private fb: FormBuilder,
    private guideService: GuideService,
    private message: NzMessageService,
    private userService: UserProfileService
  ) {
    this.navLinks = getCmsNavLinks(null);
  }

  ngOnInit(): void {
    this.guideForm = this.fb.group({
      title: this.fb.control('', [Validators.required]),
      city: this.fb.control(''),
      comment: this.fb.control('', [Validators.maxLength(5000)]),
      audioLink: this.fb.control(''),
      imageLink: this.fb.control('')
    });
  
    this.userService.getUserProfile().subscribe((t) => {
      if (t?.id) {
        this.currentUserId = t.id;
      }
    });
  }

  onFileUploaded(result: FileUploadResult) {
    if (result.fileType === FileType.AUDIO) {
      this.guideForm.controls['audioLink'].setValue(result.fileKey);
    } else {
      this.guideForm.controls['imageLink'].setValue(result.fileKey);
    }
  }

  submitForm(): void {
    this.isOkLoading = true;
    const loadingMessageId = this.message.loading('Creating place...', {
      nzDuration: 0,
    }).messageId;
    if (this.guideForm.valid) {
      const formValues = this.guideForm.value;

      const requestPayload = {
        title: formValues.title, 
        description: formValues.description,
        city: formValues.city,
        moneyAmount: formValues.price.moneyAmount, 
        currencyCode: formValues.price.currencyCode,
        languageCode: formValues.language,
        authorId: this.currentUserId,
        datePublished: formValues.datePublished,
        audioLink: formValues.audioLink,
        imageLink: formValues.imageLink,
        placeIds: this.editMapComponent.createdPlaces.map(place => place.id)
      };

      console.log('submit', this.guideForm.value);

      console.log('submit payload', requestPayload);
      this.guideService.createGuide(requestPayload).subscribe({
        next: (res) => {
          // Remove loading message
          this.message.remove(loadingMessageId);
          // Display success message
          this.message.success('Guide created successfully!');
        },
        error: (err) => {
          console.error('Error creating guide:', err);
          // Remove loading message
          this.message.remove(loadingMessageId);
          // Display error message
          this.message.error('Error creating guide.');
          this.isOkLoading = false;
        },
      });
    } else {
      Object.values(this.guideForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
