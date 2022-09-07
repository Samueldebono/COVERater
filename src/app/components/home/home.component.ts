import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserCreate, UserForLogin } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less'],
})
export class HomeComponent implements OnInit {
  constructor(private router: Router, private userService: UserService) {}

  ngOnInit(): void {}

  start() {
    this.userService
      .getUser(localStorage.getItem('id'))
      .subscribe((userStats) => {
        const phase =
          userStats !== undefined && userStats !== null
            ? userStats.phase + 1
            : 1;
        var userCreate: UserCreate = {
          roleId: localStorage.getItem('id'),
          phase: phase,
        };
        //create new userStats
        this.userService.createUser(userCreate).subscribe((userModel) => {
          this.router.navigate(['/stageOne/' + localStorage.getItem('id')], {
            state: { data: { userModel } },
          });
        });
      });
  }
}
