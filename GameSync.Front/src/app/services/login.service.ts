import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, of} from "rxjs";
import {environment} from "../../environments/environment";
import {Me, User} from "../models/models";

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) {
  }

  signUp(user: User): Observable<User> {
    return this.http.post<User>(`${environment.apiUrl}/users/sign-up`, user);
  }

  signIn(user: User): Observable<User> {
    return this.http.post<User>(`${environment.apiUrl}/users/sign-in`, user);
  }

  changePassword(request: {email: string, token: string, password: string, confirmPassword: string}) : Observable<never> {
    return this.http.post<never>(`${environment.apiUrl}/users/change-password`, request);
  }

  sendForgotPasswordRequest(email: string): Observable<never> {
    return this.http.post<never>(`${environment.apiUrl}/users/forgot-password`, {email});
  }

  getMe(): Observable<Me> {
    return this.http.get<Me>(`${environment.apiUrl}/users/me`);
  }

}
