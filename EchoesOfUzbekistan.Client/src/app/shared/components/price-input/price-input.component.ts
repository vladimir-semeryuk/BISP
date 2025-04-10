import {
  Component,
  forwardRef,
  inject,
  Input,
  OnInit,
} from '@angular/core';
import {
  FormGroup,
  ControlContainer,
  FormControl,
  Validators,
  ReactiveFormsModule,
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  FormBuilder,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzFormModule } from 'ng-zorro-antd/form';
import { getCustomValidateStatus } from '../../../desktop-app/components/screens/d-signup-screen/temporary-util';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-price-input',
  imports: [
    NzSelectModule,
    NzInputNumberModule,
    ReactiveFormsModule,
    NzFormModule,
    CommonModule,
  ],
  templateUrl: './price-input.component.html',
  styleUrl: './price-input.component.less',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => PriceInputComponent),
      multi: true,
    },
  ],
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => inject(ControlContainer, { skipSelf: true }),
    },
  ],
})
export class PriceInputComponent implements OnInit, ControlValueAccessor {
  @Input({ required: true }) controlTitle = '';
  @Input({ required: true }) id = '';
  @Input() initialValue = 0;
  @Input() required: boolean = true;
  @Input() customFieldsSpan = 20;
  @Input() customLabelSpan = 5;

  parentContainer = inject(ControlContainer);

  get parentFormContainer(): FormGroup {
    return this.parentContainer.control as FormGroup;
  }

  get valueControl(): FormControl {
    const valueControl = this.priceForm.get('value') as FormControl;
    return valueControl;
  }

  get valueControlStatus(): string {
    return getCustomValidateStatus(this.valueControl);
  }

  priceForm!: FormGroup;

  // Callbacks to pass changes/touches.
  onChange: any = () => {};
  onTouched: any = () => {};

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.priceForm = this.fb.group(
      {
        moneyAmount: [this.initialValue, [Validators.required, Validators.min(0)]],
        currencyCode: ['UZS', Validators.required],
      },
      { validators: this.priceValidator }
    );
    this.parentFormContainer.addControl(this.controlTitle, this.priceForm);

    // pass changes from the internal form group.
    this.priceForm.valueChanges.subscribe((value) => {
      this.onChange({ value });
    });
  }

  // Custom validator for the whole form group
  priceValidator(formGroup: AbstractControl): ValidationErrors | null {
    const valueControl = formGroup.get('moneyAmount');
    const currencyControl = formGroup.get('currencyCode');

    if (valueControl?.invalid || currencyControl?.invalid) {
      return { invalidPrice: true };
    }
    return null;
  }

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

