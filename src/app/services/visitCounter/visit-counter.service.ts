import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class VisitCounter {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  getUpdateVisitCounter(): Observable<number> {
    const httpOptions = {
      headers: new HttpHeaders().set('Content-Type', 'application/json;'),
    };

    return this.http.post<number>(
      this.baseUrl + '/V1/visitCounter',
      httpOptions
    );
  }
}
