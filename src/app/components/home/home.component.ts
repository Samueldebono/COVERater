import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Helper } from 'src/app/services/helper.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less'],
})
export class HomeComponent implements OnInit {
  constructor(
    private router: Router,
    private userService: UserService,
    private helper: Helper
  ) {}
  loading: boolean = true;
  completed: boolean = false;

  ngOnInit(): void {
    this.helper.sessionCheck();
  }
  onLoad() {
    this.loading = false;
  }

  start() {
    if (this.helper.sessionCheck()) {
      this.userService
        .getUser(localStorage.getItem('id'))
        .subscribe((userStats) => {
          if (userStats !== undefined) {
            const userModel = userStats;
            this.router.navigate(
              ['/quiz/' + localStorage.getItem('id') + '/' + userStats.userId],
              {
                state: { data: { userModel } },
              }
            );
          }
        });
    }
  }
}
