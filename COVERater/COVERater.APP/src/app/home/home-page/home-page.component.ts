import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.less']
})
export class HomePageComponent implements OnInit {

  constructor(private router: Router) {
  }

  ngOnInit(): void {
  }

  startQuiz(){
    this.router.navigate([ '/quizLoader']);
  }

}
