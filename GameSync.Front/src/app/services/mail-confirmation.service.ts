import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, EMPTY } from 'rxjs';
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class MailConfirmationService {
  constructor(private http: HttpClient) {
  }

  sendConfirmationToken(confirmationToken: string, email: string): Observable<never> {
    return this.http.post<never>(`${environment.apiUrl}/users/confirm`, {
      confirmationToken,
      email
    })
  }

}
