import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { UserGuessModel } from '../models/userGuess.model';
import { AlertifyService } from './alertify/alertify.service';

@Injectable({
  providedIn: 'root',
})
export class Helper {
  constructor(private alertify: AlertifyService, private router: Router) {}
  findAverageDifferenceOfList(list: UserGuessModel[]) {
    var dif = 0.0;
    if (list.length > 0) {
      list.forEach((guess) => {
        dif += this.findSingleDifference(guess);
      });

      dif = (1 - dif / list.length) * 100;
    }
    return Math.round((dif + Number.EPSILON) * 100) / 100;
  }

  findAverageDifferenceOfListLastTen(list: UserGuessModel[]) {
    var dif = 0.0;
    if (list.length > 0) {
      list.sort((x, y) => y.usersGuessId - x.usersGuessId);
      list.splice(10);
      list.forEach((guess) => {
        dif += this.findSingleDifference(guess);
      });

      dif = (1 - dif / list.length) * 100;
    }
    return Math.round((dif + Number.EPSILON) * 100) / 100;
  }

  findSingleDifference(guess: UserGuessModel) {
    var calc = guess.guessPercentage - guess.subImage.coverageRate;
    var dif = calc < 0 ? calc * -1 : calc;
    return dif;
  }

  tokenExpired(token: string) {
    if (token === null || token === undefined) {
      return true;
    } else {
      const expiry = JSON.parse(atob(token.split('.')[1])).exp;
      return Math.floor(new Date().getTime() / 1000) >= expiry;
    }
  }

  onLogout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userName');
    localStorage.removeItem('status');
    localStorage.removeItem('role');
    this.alertify.success('Thanks for stopping by');
    this.router.navigate(['/login'], {});
  }

  sessionCheck() {
    if (this.tokenExpired(localStorage.getItem('token'))) {
      this.onLogout();
      return false;
    } else return true;
  }

  adminCheck() {
    if (localStorage.getItem('role') === '4') {
      this.sessionCheck();
      return true;
    } else {
      this.router.navigate(['/home'], {});
      return false;
    }
  }
}
