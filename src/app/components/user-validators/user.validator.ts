import {
  AbstractControl,
  AsyncValidatorFn,
  ValidationErrors,
} from '@angular/forms';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserForLoginForgotPassword } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/user/authentication.service';

export class UserValidator {
  static createValidator(userService: AuthService): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors> => {
      return userService
        .userExists(control.value)
        .pipe(
          map((result: boolean) =>
            result !== null && result ? { usernameAlreadyExists: result } : null
          )
        );
    };
  }
}
