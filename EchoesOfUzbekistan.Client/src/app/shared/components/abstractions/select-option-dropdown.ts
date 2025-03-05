// select-option-dropdown.component.ts
import { OnInit, OnDestroy, Input, Directive, inject } from '@angular/core';
import { ControlContainer, FormControl, FormGroup } from '@angular/forms';
import { ControlValueAccessor } from '@angular/forms';

@Directive()
export abstract class SelectOptionDropdownComponent<T> implements OnInit, OnDestroy, ControlValueAccessor {
  @Input({ required: true }) controlTitle = '';
  @Input({required: true}) id = ''
  @Input() required: boolean = false;
  options: T[] = [];
  formControl!: FormControl;
  parentContainer = inject(ControlContainer);

  // ControlValueAccessor callbacks.
  onChange: any = () => {};
  onTouched: any = () => {};

  // Inject the parent form container.
  get parentFormGroup(): FormGroup {
    return this.parentContainer.control as FormGroup;
  }
  abstract loadOptions(): void;
  abstract setValidators(required: boolean, formControl: FormControl<T | null>): void;

  ngOnInit(): void {
    this.formControl = new FormControl<T | null>(null);
    this.parentFormGroup.addControl(this.controlTitle, this.formControl);

    this.setValidators(this.required, this.formControl);
    
    this.loadOptions();

    this.formControl.valueChanges.subscribe(value => this.onChange(value));
  }

  ngOnDestroy(): void {
    // Remove control when component is destroyed.
    this.parentFormGroup.removeControl(this.controlTitle);
  }

  writeValue(value: any): void {
    this.formControl.setValue(value);
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState(isDisabled: boolean): void {
    isDisabled ? this.formControl.disable() : this.formControl.enable();
  }
}
