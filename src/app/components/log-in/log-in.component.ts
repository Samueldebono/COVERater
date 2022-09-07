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
import { UserService } from 'src/app/services/user/user.service';

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
    private userService: UserService,
    private alertify: AlertifyService,
    private router: Router,
    private route: ActivatedRoute
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
    // const token = this.authService.authUser(loginForm.value);
    this.userSubmitted = true;
    if (this.loginForm.valid) {
      this.authService.authUser(this.loginForm.value).subscribe(
        (response) => {
          const user: UserForLogin = response;

          if (user) {
            localStorage.setItem('token', user.bearerToken);
            localStorage.setItem('userName', user.userName);
            localStorage.setItem('id', user.roleId.toString());
            localStorage.setItem('role', user.roleType.toString());
            this.alertify.success('Login Successful');
            if (user.roleType == 1 || user.userStats.length > 0) {
              this.router.navigate(['/home/'], { state: { data: { user } } });
            } else if (user.roleType == 2) {
              this.router.navigate(['/experience/'], {
                state: { data: { user } },
              });
            } else {
              this.router.navigate(['/home/'], { state: { data: { user } } });
            }
          }
        },
        (error) => {
          // handle error
          this.alertify.error('Incorrect Login or Password');
        }
      );
    }
  }

  userData(): UserForLogin {
    return (this.user = {
      userName: this.userName.value,
      password: this.password.value,
      bearerToken: '',
      status: 0,
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
