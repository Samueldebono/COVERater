import { Component, OnInit } from '@angular/core';
import { Helper } from 'src/app/services/helper.service';
import { VisitCounter } from 'src/app/services/visitCounter/visit-counter.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.less'],
})
export class NavBarComponent implements OnInit {
  loggedinUser?: string;
  adminLoggedinUser?: boolean;
  status?: string;
  constructor(private helper: Helper, private visitCounter: VisitCounter) {}

  counter: number = 1;

  ngOnInit() {
    this.visitCounter.getUpdateVisitCounter().subscribe((counter) => {
      this.counter = counter;
    });
  }

  loggedin() {
    this.loggedinUser = localStorage.getItem('userName');
    return this.loggedinUser;
  }

  isActivated() {
    this.status = localStorage.getItem('status');
    return this.status == '2'; //activated
  }

  onLogout() {
    this.helper.onLogout();
  }
}
