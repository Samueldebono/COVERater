import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExperienceType } from 'src/app/enums/enums';
import { UserForLogin, UserForRegister } from 'src/app/models/user.model';
import { AlertifyService } from 'src/app/services/alertify/alertify.service';
import { AuthService } from 'src/app/services/user/authentication.service';
import { UserValidator } from '../user-validators/user.validator';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less'],
})
export class RegisterComponent implements OnInit {
  xp: any = ['>5 years', '3-5 years', '1-3 years', 'student'];
  registerationForm: FormGroup;
  userSubmitted: boolean;
  countryList: any[];
  stateList: any[];
  userCheck: { email: string };
  companyId: string;
  showClientFields: boolean;
  user: UserForRegister;
  userLogin: UserForLogin;
  userRole: number;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authServices: AuthService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.createRegisterationForm();
    this.userRole = this.route.snapshot.params['role'];
  }

  createRegisterationForm() {
    this.registerationForm = this.fb.group({
      // userName: [null, Validators.required],
      email: [
        null,
        [Validators.required, Validators.email],
        [UserValidator.createValidator(this.authServices)],
      ],
    });
  }

  onSubmit() {
    this.userSubmitted = true;
    if (this.registerationForm.valid) {
      this.authServices.registerUser(this.userData()).subscribe((member) => {
        this.onReset();
        this.alertify.success('Congrats, you are successfully registered');
        this.router.navigate(['/login/1']);
      });
    } else {
      this.alertify.error(
        'Please review the form and provide all valid entries'
      );
    }
  }

  onReset() {
    this.userSubmitted = false;
    this.registerationForm.reset();
  }

  onLogin(email: string) {
    this.router.navigate(['/user/login'], { state: { data: { email } } });
  }

  userData(): UserForRegister {
    return (this.user = {
      userName: this.email.value,
      email: this.email.value,
      password: '',
      experience: ExperienceType.student,
      roleType: this.userRole,
    });
  }

  get email() {
    return this.registerationForm.get('email') as FormControl;
  }
  get password() {
    return this.registerationForm.get('password') as FormControl;
  }
  get confirmPassword() {
    return this.registerationForm.get('confirmPassword') as FormControl;
  }
  get experience() {
    return this.registerationForm.get('experience') as FormControl;
  }
}
