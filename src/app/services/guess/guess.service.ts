import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Guess } from 'src/app/models/guess.model';
import { Images } from 'src/app/models/image.model';
import { UserForLogin } from 'src/app/models/user.model';
import { UserGuessModel } from 'src/app/models/userGuess.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class GuessService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  saveGuess(guess: Guess): Observable<boolean> {
    const guessJson = JSON.stringify(guess);
    let auth_token = localStorage.getItem('token');

    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.post<boolean>(
      this.baseUrl + '/V1/usersGuess',
      guessJson,
      requestOptions
    );
  }

  getGuesses(): Observable<UserGuessModel[]> {
    let auth_token = localStorage.getItem('token');
    let id = localStorage.getItem('id');
    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.get<UserGuessModel[]>(
      this.baseUrl + '/V1/usersGuess/' + id + '/1',
      requestOptions
    );
  }
}
