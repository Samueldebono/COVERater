import { Injectable } from '@angular/core';
import {
  UpdateUser,
  UserCreate,
  UserForLoginForgotPassword,
  UserModel,
} from 'src/app/models/user.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  updateUser(updateUser: any, id?: any): Observable<UserModel> {
    const updateUserJson = JSON.stringify(updateUser);

    let auth_token = localStorage.getItem('token');

    let userId = localStorage.getItem('id');

    if (id !== undefined) {
      userId = id;
    }
    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.post<UserModel>(
      this.baseUrl + '/V1/user/' + userId,
      updateUserJson,
      requestOptions
    );
  }

  createUser(UserCreate: UserCreate): Observable<UserModel> {
    const updateUserJson = JSON.stringify(UserCreate);

    let auth_token = localStorage.getItem('token');
    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.post<UserModel>(
      this.baseUrl + '/V1/user/',
      updateUserJson,
      requestOptions
    );
  }

  getUser(id: string): Observable<UserModel> {
    let auth_token = localStorage.getItem('token');
    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.get<UserModel>(
      this.baseUrl + '/V1/user/' + id,
      requestOptions
    );
  }
}
