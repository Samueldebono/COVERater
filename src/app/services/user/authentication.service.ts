import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  UserForLogin,
  UserForLoginForgotPassword,
  UserForRegister,
} from '../../models/user.model';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

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
    const httpOptions = {
      //new HttpHeaders({
      headers:
        //   // Authorization: 'Bearer ' + localStorage.getItem('token'),
        //   // ContentType: 'application/json',
        // }),
        new HttpHeaders().set('Content-Type', 'application/json;'),
    };
    return this.http.post<UserForRegister>(
      this.baseUrl + '/V1/authUser',
      userReg,
      httpOptions
    );
  }
  resetPassword(forgotPassword: UserForLoginForgotPassword): Observable<any> {
    const forgotPasswordJson = JSON.stringify(forgotPassword);

    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      // Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.post<any>(
      this.baseUrl + '/V1/reset/password',
      forgotPasswordJson,
      requestOptions
    );
  }

  userExists(email: string): Observable<boolean> {
    // const httpOptions = {
    //   //new HttpHeaders({
    //   headers:
    //     //   // Authorization: 'Bearer ' + localStorage.getItem('token'),
    //     //   // ContentType: 'application/json',
    //     // }),
    //     new HttpHeaders().set('Content-Type', 'application/json;'),
    // };
    return this.http.get<boolean>(
      this.baseUrl + '/v1/authUserExists?email=' + email
    );
  }

  sendDetails(id: string): Observable<boolean> {
    let auth_token = localStorage.getItem('token');
    console.log('TOKEN: ' + auth_token);
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
}
