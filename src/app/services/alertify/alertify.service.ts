import { Injectable } from '@angular/core';
import * as alertyfy from 'alertifyjs';

@Injectable({
  providedIn: 'root',
})
export class AlertifyService {
  constructor() {}

  success(message: string) {
    alertyfy.success(message, 5);
  }

  warning(message: string) {
    alertyfy.warning(message, 5);
  }

  error(message: string) {
    alertyfy.error(message, 5);
  }
}
