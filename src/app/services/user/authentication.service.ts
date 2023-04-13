import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  UserExperienceUpdate,
  UserForLogin,
  UserForLoginForgotPassword,
  UserForRegister,
} from '../../models/user.model';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { authUser, authUserWithUserStats } from 'src/app/models/authUser.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  authUser(user: UserForLogin): Observable<UserForLogin> {
    const login = JSON.stringify(user);
    const httpOptions = {
      headers: new HttpHeaders().set('Content-Type', 'application/json;'),
    };

    return this.http.post<UserForLogin>(
      this.baseUrl + '/V1/authenticate',
      login,
      httpOptions
    );
  }

  registerUser(user: UserForRegister): Observable<UserForRegister> {
    const userReg = JSON.stringify(user);
    let auth_token = localStorage.getItem('token');
    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.post<UserForRegister>(
      this.baseUrl + '/V1/authUser',
      userReg,
      requestOptions
    );
  }

  getUserExperence(id: string): Observable<authUser> {
    let auth_token = localStorage.getItem('token');
    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.get<authUser>(
      this.baseUrl + '/V1/authUser/experience/' + id,
      requestOptions
    );
  }
}
