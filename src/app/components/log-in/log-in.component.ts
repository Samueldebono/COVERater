import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserCreate, UserForLogin } from 'src/app/models/user.model';
import { AlertifyService } from 'src/app/services/alertify/alertify.service';
import { AuthService } from 'src/app/services/user/authentication.service';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.less'],
})
export class LogInComponent implements OnInit {
  loginForm: FormGroup;
  user: UserForLogin;
  userSubmitted: boolean;
  passedEmail: string;
  reRouted: boolean;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private alertify: AlertifyService,
    private router: Router
  ) {}

  ngOnInit(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('userName');
    localStorage.removeItem('id');
    localStorage.removeItem('role');
    this.createLoginForm();
  }

  createLoginForm() {
    this.loginForm = this.fb.group({
      userName: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required]],
    });
  }

  onSubmit() {
    this.userSubmitted = true;
    this.authService.authUser(this.loginForm.value).subscribe(
      (response) => {
        const user: UserForLogin = response;

        if (user) {
          localStorage.setItem('token', user.bearerToken);
          localStorage.setItem('userName', user.userName);
          localStorage.setItem('id', user.roleId.toString());
          localStorage.setItem('role', user.roleType.toString());
          this.alertify.success('Welcome!');
          this.router.navigate(['/home/'], { state: { data: { user } } });
        }
      },
      (error) => {
        // handle error
        this.alertify.error('Something went wrong, please contact support');
      }
    );
  }

  userData(): UserForLogin {
    return (this.user = {
      userName: this.userName.value,
      password: this.password.value,
      bearerToken: '',
      status: 0,
      guesses: [],
    });
  }

  resetPassword() {
    this.router.navigate(['/forgotPassword/']);
  }

  get userName() {
    return this.loginForm.get('userName') as FormControl;
  }

  get password() {
    return this.loginForm.get('password') as FormControl;
  }
}
