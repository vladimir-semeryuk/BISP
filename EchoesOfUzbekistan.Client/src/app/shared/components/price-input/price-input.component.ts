import { Component, forwardRef, inject, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, ControlContainer, FormControl, Validators, ReactiveFormsModule, ControlValueAccessor, NG_VALUE_ACCESSOR, FormBuilder, AbstractControl, ValidationErrors } from '@angular/forms';
import { NzSelectModule } from 'ng-zorro-antd/select';
import {NzInputNumberModule} from 'ng-zorro-antd/input-number';
import { NzFormModule } from 'ng-zorro-antd/form';
import { getCustomValidateStatus } from '../../../desktop-app/components/screens/d-signup-screen/temporary-util';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-price-input',
  imports: [NzSelectModule, NzInputNumberModule, ReactiveFormsModule, NzFormModule, CommonModule],
  templateUrl: './price-input.component.html',
  styleUrl: './price-input.component.less',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => PriceInputComponent),
      multi: true
    }
  ],
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => inject(ControlContainer, {skipSelf: true})
    }
      ],
})
export class PriceInputComponent implements OnInit, ControlValueAccessor {
  @Input({ required: true }) controlTitle = '';
  @Input({required: true}) id = ''
  @Input() initialValue = 0
  @Input() required: boolean = true;
  @Input() customFieldsSpan = 20;
  @Input() customLabelSpan = 5;

  parentContainer = inject(ControlContainer)

  get parentFormContainer(): FormGroup {
    return this.parentContainer.control as FormGroup;
  }


  get valueControl(): FormControl {
    const valueControl = this.priceForm.get('value') as FormControl
    return valueControl;
  }

  get valueControlStatus(): string {
    return getCustomValidateStatus(this.valueControl)
  }

  priceForm!: FormGroup;

  // Callbacks to propagate changes/touches.
  onChange: any = () => {};
  onTouched: any = () => {};

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.priceForm = this.fb.group({
      value: [this.initialValue, [Validators.required, Validators.min(0)]],
      currency: ['UZS', Validators.required]
    }, {validators: this.priceValidator});
    this.parentFormContainer.addControl(this.controlTitle, 
      this.priceForm
    )

    // Propagate any changes from the internal form group.
    this.priceForm.valueChanges.subscribe((value) => {
      this.onChange(value);
    });
  }

  // Custom validator for the whole form group
priceValidator(formGroup: AbstractControl): ValidationErrors | null {
  const valueControl = formGroup.get('value');
  const currencyControl = formGroup.get('currency');

  if (valueControl?.invalid || currencyControl?.invalid) {
    return { invalidPrice: true };
  }
  return null;
}

  // Optional: Allow an initial value to be set via an @Input.
  // (If needed, add: @Input() initialValue = 0;)
  // private initialValue = 0;

  // --- ControlValueAccessor Methods ---
  writeValue(value: any): void {
    if (value !== undefined && value !== null) {
      this.priceForm.setValue(value, { emitEvent: false });
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    isDisabled ? this.priceForm.disable() : this.priceForm.enable();
  }
}



// @Component({
//   selector: 'app-price-input',
//   imports: [NzSelectModule, NzInputNumberModule, ReactiveFormsModule],
//   viewProviders: [
//     {
//       provide: ControlContainer,
//       useFactory: () => inject(ControlContainer, {skipSelf: true})
//     }
//       ],
//   templateUrl: './price-input.component.html',
//   styleUrl: './price-input.component.less'
// })
// export class PriceInputComponent implements OnInit, OnDestroy, ControlValueAccessor {
//   @Input({ required: true }) controlTitle = '';
//   @Input({required: true}) id = ''
//   @Input() initialValue = 0
//   @Input() required: boolean = true;
//   formControl!: FormGroup;

//   // ControlValueAccessor callbacks.
//   onChange: any = () => {};
//   onTouched: any = () => {};

//   parentContainer = inject(ControlContainer)

//   get parentFormContainer(): FormGroup {
//     return this.parentContainer.control as FormGroup;
//   }

//   ngOnInit(): void {
//     this.parentFormContainer.addControl(this.controlTitle, 
//       new FormGroup({
//         value: new FormControl(this.initialValue, Validators.min(0)),
//         currency: new FormControl('UZS', Validators.required)
//       })
//     )
//     this.formControl.valueChanges.subscribe(value => this.onChange(value));
//   }

//   ngOnDestroy(): void {
//     this.parentFormContainer.removeControl(this.controlTitle)
//   }

//   writeValue(value: any): void {
//     this.formControl.setValue(value);
//   }
//   registerOnChange(fn: any): void {
//     this.onChange = fn;
//   }
//   registerOnTouched(fn: any): void {
//     this.onTouched = fn;
//   }
//   setDisabledState(isDisabled: boolean): void {
//     isDisabled ? this.formControl.disable() : this.formControl.enable();
//   }
// }
