import {AbstractControl, ValidationErrors, ValidatorFn} from "@angular/forms";

export function passwordMatchValidator(passwordKey: string): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    return control.parent?.get(passwordKey)?.value === control.value ? null : {passwordNoMatch: true}
  }
}
