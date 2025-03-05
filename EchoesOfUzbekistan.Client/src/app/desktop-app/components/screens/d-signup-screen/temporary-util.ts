import { AbstractControl, FormControl } from '@angular/forms';

export function getCustomValidateStatus(control: AbstractControl | null): 'success' | 'warning' | 'error' | 'validating' | '' {
  if (!control) return '';
  if (control.invalid && (control.dirty || control.touched)) {
    return 'error';
  } else if (control.valid && (control.dirty || control.touched)) {
    return 'success';
  }
  return '';
}