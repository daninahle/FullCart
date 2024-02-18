import { FormControl, FormGroup,AbstractControl, ValidatorFn } from '@angular/forms';

export default class ValidateForm {
  static validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control instanceof FormControl) {
        control.markAsDirty({ onlySelf: true });
      } else if (control instanceof FormGroup) {
        this.validateAllFormFields(control);
      }
    });
  }

  static passwordStrengthValidator(minLength: number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const password: string = control.value;
  
      if (!password) {
        return null; // No validation error if the password is empty
      }
  
      // Add your custom password strength criteria here
      const isStrong = password.length >= minLength;
  
      return isStrong ? null : { 'weakPassword': true };
    };
  }

}



