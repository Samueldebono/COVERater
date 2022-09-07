import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from '../../services/alertify/alertify.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.less'],
})
export class NavBarComponent implements OnInit {
  loggedinUser?: string;
  adminLoggedinUser?: boolean;
  status?: string;
  constructor(private alertify: AlertifyService, private router: Router) {}

  ngOnInit() {}

  loggedin() {
    this.loggedinUser = localStorage.getItem('userName');
    return this.loggedinUser;
  }

  isActivated() {
    this.status = localStorage.getItem('status');
    return this.status == '2'; //activated
  }

  onLogout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userName');
    localStorage.removeItem('status');
    localStorage.removeItem('role');
    this.alertify.success('You are logged out!');
    this.router.navigate(['/login'], {});
  }

  adminLoggedin() {
    this.adminLoggedinUser =
      localStorage.getItem('userName') && localStorage.getItem('role') === '4';
    return this.adminLoggedinUser;
  }
}
