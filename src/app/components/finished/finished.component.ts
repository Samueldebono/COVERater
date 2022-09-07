import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { UserModel } from 'src/app/models/user.model';
import { UserGuessModel } from 'src/app/models/userGuess.model';
import { GuessService } from 'src/app/services/guess/guess.service';
import { UserService } from 'src/app/services/user/user.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/user/authentication.service';

@Component({
  selector: 'app-finished',
  templateUrl: './finished.component.html',
  styleUrls: ['./finished.component.less'],
})
export class FinishedComponent implements OnInit {
  finishForm: FormGroup;
  totalTime: string;
  averageTime: string;
  data: any[];
  userGuesses: UserGuessModel[];
  user: UserModel;
  userId: any;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private guessService: GuessService,
    private location: Location,
    private router: Router
  ) {
    this.userId = this.location.getState();
    console.log(this.userId);
  }
  // Array of different segments in chart
  lineChartData: ChartDataSets[] = [{ data: [], label: 'Product A' }];

  //Labels shown on the x-axis
  lineChartLabels: Label[] = [];

  // Define chart options
  lineChartOptions: ChartOptions = {
    responsive: true,
  };

  // Define colors of chart segments
  lineChartColors: Color[] = [
    {
      // dark grey
      backgroundColor: 'rgba(77,83,96,0.2)',
      borderColor: 'rgba(77,83,96,1)',
    },
    {
      // red
      backgroundColor: 'rgba(255,0,0,0.3)',
      borderColor: 'red',
    },
  ];

  // Set true to show legends
  lineChartLegend = true;

  // Define type of chart
  lineChartType: ChartType = 'line';

  lineChartPlugins: any = [];

  ngOnInit() {
    this.finishForm = this.fb.group({});
    this.calculate();
  }

  onSubmit() {
    this.router.navigate(['/home/']);
  }

  sendDetails() {
    this.authService
      .sendDetails(localStorage.getItem('id'))
      .subscribe((results) => {});
  }

  calculate() {
    this.userService
      .getUser(localStorage.getItem('id'))
      .subscribe((results) => {
        this.user = results;

        this.guessService.getGuesses().subscribe((guesses) => {
          this.userGuesses = guesses;

          var imageNumberArray: any[] = [];
          var guessResultsArray: any[] = [];

          var time =
            this.user.timePhase2 != null || undefined
              ? this.user.timePhase2
              : new Date();

          //convert time into seconds
          var seconds = new Date(time).getSeconds();
          seconds += new Date(time).getMinutes() * 60;
          seconds += new Date(time).getHours() * 3600;

          //Get GuessPercentage - Actual Answer for table
          let i: number = 1;
          var guesses = this.userGuesses.sort((x) => x.usersGuessId);
          guesses.forEach((item) => {
            var result =
              (item.guessPercentage - item.subImage.coverageRate) * 100;
            guessResultsArray.push(result);
            imageNumberArray.push(i);
            i++;
          });

          //Total Time
          this.totalTime = this.convertHMS(seconds);

          //Average Time
          var average = seconds / imageNumberArray.length;
          this.averageTime = this.convertHMS(average);
          this.lineChartData[0].data = guessResultsArray;
          this.lineChartLabels = imageNumberArray;
        });
      });
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
  // events
  chartClicked({ event, active }: { event: MouseEvent; active: {}[] }): void {
    console.log(event, active);
  }

  chartHovered({ event, active }: { event: MouseEvent; active: {}[] }): void {
    console.log(event, active);
  }
}