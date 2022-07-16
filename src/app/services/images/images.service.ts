import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Images } from 'src/app/models/image.model';
import { UserForLogin } from 'src/app/models/user.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ImagesService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  getAllImages(): Observable<Images[]> {
    let auth_token = localStorage.getItem('token');

    const httpOptions = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${auth_token}`,
    });
    const requestOptions = { headers: httpOptions };
    return this.http.get<Images[]>(this.baseUrl + '/V1/images', requestOptions);
  }
}
