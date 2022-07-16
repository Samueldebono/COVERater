import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserForLoginForgotPassword } from 'src/app/models/user.model';
import { AlertifyService } from 'src/app/services/alertify/alertify.service';
import { AuthService } from 'src/app/services/user/authentication.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.less'],
})
export class ForgotPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  user: UserForLoginForgotPassword;
  userSubmitted: boolean;
  passedEmail: string;
  reRouted: boolean;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private alertify: AlertifyService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.createResestForm();
    //passed from register page
    this.passedEmail = history.state.data.email;
  }

  createResestForm() {
    this.resetPasswordForm = this.fb.group({
      email: [null, [Validators.required, Validators.email]],
    });
  }

  onSubmit() {
    // const token = this.authService.authUser(loginForm.value);
    this.userSubmitted = true;
    if (this.resetPasswordForm.valid) {
      this.authService
        .resetPassword(this.resetPasswordForm.value)
        .subscribe((response) => {
          this.alertify.success('Email Sent');
          this.router.navigate(['/login/1']);
        });
    }
  }

  userData(): UserForLoginForgotPassword {
    return (this.user = {
      email: this.email.value,
    });
  }

  get email() {
    return this.resetPasswordForm.get('email') as FormControl;
  }
}
