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
import { authUser } from 'src/app/models/authUser.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  authUser(user: UserForLogin): Observable<UserForLogin> {
    const login = JSON.stringify(user);
    const httpOptions = {
      //new HttpHeaders({
      headers:
        //   // Authorization: 'Bearer ' + localStorage.getItem('token'),
        //   // ContentType: 'application/json',
        // }),
        new HttpHeaders().set('Content-Type', 'application/json;'),
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

  updateExperience(user: UserExperienceUpdate): Observable<boolean> {
    const userReg = JSON.stringify(user);
    let auth_token = localStorage.getItem('token');
    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.post<boolean>(
      this.baseUrl + '/V1/authUser/experience',
      userReg,
      requestOptions
    );
  }

  resetPassword(forgotPassword: UserForLoginForgotPassword): Observable<any> {
    const forgotPasswordJson = JSON.stringify(forgotPassword);
    let auth_token = localStorage.getItem('token');
    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.post<any>(
      this.baseUrl + '/V1/reset/password',
      forgotPasswordJson,
      requestOptions
    );
  }

  userExists(email: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders().set('Content-Type', 'application/json;'),
    };
    return this.http.get<boolean>(
      this.baseUrl + '/v1/authUserExists?email=' + email
    );
  }

  sendDetails(id: string): Observable<boolean> {
    let auth_token = localStorage.getItem('token');
    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.get<boolean>(
      this.baseUrl + '/V1/sendDetails/' + id,
      requestOptions
    );
  }

  getUsers(): Observable<authUser[]> {
    let auth_token = localStorage.getItem('token');
    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.get<authUser[]>(
      this.baseUrl + '/V1/authUsers/results/',
      requestOptions
    );
  }
}
