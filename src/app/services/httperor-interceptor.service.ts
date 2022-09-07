import {
  HttpErrorResponse,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, concatMap, retry, retryWhen } from 'rxjs/operators';
import { ErrorCode } from '../enums/enums';
import { AlertifyService } from './alertify/alertify.service';

@Injectable({
  providedIn: 'root',
})
export class HttpErrorInterceptorService implements HttpInterceptor {
  constructor(private alertify: AlertifyService) {}
  intercept(request: HttpRequest<any>, next: HttpHandler) {
    return next.handle(request).pipe(
      retryWhen((error) => this.retryRequest(error, 5)),
      catchError((error: HttpErrorResponse) => {
        const errorMessage = this.setError(error);
        // this.alertify.error(errorMessage);
        return throwError(errorMessage);
      })
    );
  }

  // Retry the request in case of errror
  retryRequest(
    error: Observable<any>,
    retryCount: number
  ): Observable<unknown> {
    return error.pipe(
      concatMap((checkErr: HttpErrorResponse, count: number) => {
        if (count <= retryCount) {
          switch (checkErr.status) {
            case ErrorCode.serverDown:
              return of(checkErr);
            case ErrorCode.unauthorised:
              return of(checkErr);
          }
        }
        return throwError(checkErr);
      })
    );
  }

  setError(error: HttpErrorResponse): string {
    let errorMessage = 'Unknown error occured';
    if (error.error instanceof ErrorEvent) {
      // Client side error
      errorMessage = error.error.message;
    } else {
      // server side error
      if (error.status === 401) {
        return error.error;
      }
      // else if (error.status === 402) {
      //   return error.error;
      // } else if (error.status === 422) {
      //   return error.error;
      // } else if (error.status === 500) {
      //   return error.error;
      // }
      if (error.error instanceof ErrorEvent) {
        // Client-side errors
        errorMessage = `Error: ${error.error.message}`;
      } else {
        // Server-side errors
        // errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        // errorMessage = `Error Code: ${error.status}\nMessage: Couldn't connect to server`;
        errorMessage = error.error;
      }

      if (error.error.errorMessage && error.status !== 0) {
        {
          errorMessage = error.error.errorMessage;
        }
      }
    }
    return errorMessage;
  }
}
