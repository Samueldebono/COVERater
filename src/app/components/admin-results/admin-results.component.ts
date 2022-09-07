import { Component, OnInit } from '@angular/core';
import { UserModel } from 'src/app/models/user.model';
import { PhaseType } from 'src/app/models/userGuess.model';
import { UserService } from 'src/app/services/user/user.service';
import { ExperienceType } from '../../enums/enums';

@Component({
  selector: 'app-admin-results',
  templateUrl: './admin-results.component.html',
  styleUrls: ['./admin-results.component.less'],
})
export class AdminResultsComponent implements OnInit {
  userDataOriginal: UserModel[] = [];
  userData: UserModel[] = [];
  categoriesHeading: string[];
  students: boolean = true;
  buttonText: string = 'Switch to Experienced Users';
  totalTime: string;

  displayedColumns: string[] = [
    'email',
    'user',
    'phase',
    'time',
    'picture',
    'averageAccuracy',
  ];

  displayedColumns2: string[] = [
    'user',
    'surveyAnswer',
    'picture',
    'time',
    'averageAccuracy',
  ];
  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getUsers().subscribe((data) => {
      data.forEach((val) => this.userData.push(Object.assign({}, val)));
      data.forEach((val) => this.userDataOriginal.push(Object.assign({}, val)));
      this.userData = this.userData.filter((x) => x.role == 1);
    });
  }
  switchStatus() {
    this.buttonText = 'Switch to Normal Users';
    this.userDataOriginal.forEach((val) =>
      this.userData.push(Object.assign({}, val))
    );
    this.students = !this.students;
    if (this.students) {
      this.userData = this.userData.filter((x) => x.role == 1);
    } else {
      this.userData = this.userData.filter((x) => x.role == 2);
    }
  }

  getTimeTaken(timestart: Date, timeFinished: Date) {
    //convert time into seconds
    var seconds = new Date(timestart).getSeconds();
    seconds += new Date(timestart).getMinutes() * 60;
    seconds += new Date(timestart).getHours() * 3600;

    var seconds2 = new Date(timeFinished).getSeconds();
    seconds2 += new Date(timeFinished).getMinutes() * 60;
    seconds2 += new Date(timeFinished).getHours() * 3600;

    seconds = seconds2 - seconds;

    //Total Time
    this.totalTime = this.convertHMS(seconds);
    return this.totalTime;
  }

  convertHMS(value: number) {
    let hours = Math.floor(value / 3600); // get hours
    let minutes = Math.floor((value - hours * 3600) / 60); // get minutes
    let seconds = value - hours * 3600 - minutes * 60; //  get seconds
    let h: string;
    let m: string;
    let s: string;
    // add 0 if value < 10; Example: 2 => 02
    if (hours < 10) {
      h = '0' + hours;
    } else {
      h = hours.toString();
    }
    if (minutes < 10) {
      m = '0' + minutes;
    } else {
      m = minutes.toString();
    }
    if (seconds < 10) {
      s = '0' + seconds;
    } else {
      s = seconds.toString();
    }
    return h.slice(0, 2) + ':' + m.slice(0, 2) + ':' + s.slice(0, 2); // Return is HH : MM : SS
  }

  returnPhase(phase: PhaseType) {
    return PhaseType[phase];
  }

  returnExp(exp: ExperienceType) {
    return ExperienceType[exp];
  }
}
