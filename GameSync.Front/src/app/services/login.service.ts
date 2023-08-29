import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, of} from "rxjs";
import {environment} from "../../environments/environment";
import {User} from "../models/models";

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

  me() : Observable<User> {
    return of({
      userName: 'babacouda',
      token: '12345',
      password: '1234',
      email: 'babacouda@gmail.com'
    });
  }
}
