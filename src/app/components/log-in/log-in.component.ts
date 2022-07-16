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
    this.createLoginForm();
    //passed from register page
    this.passedEmail = history.state.data.email;
    this.loginForm.controls['userName'].setValue(this.passedEmail);
    this.reRouted = this.route.snapshot.params['id'] == 1;
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
          console.log(response);
          const user = response;

          if (user) {
            var userCreate: UserCreate = {
              roleId: user.roleId,
            };
            //create new userStats
            this.userService.createUser(userCreate).subscribe(() => {
              localStorage.setItem('token', user.bearerToken);
              localStorage.setItem('userName', user.userName);
              localStorage.setItem('id', user.roleId);
              this.alertify.success('Login Successful');
              this.router.navigate(['/home/']);
            });
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
