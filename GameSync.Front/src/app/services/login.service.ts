import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../../environments/environment";
import {User} from "../models/models";

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) {
  }

  signUp(user: User): Observable<User> {
    return this.http.post<User>(`${environment.apiUrl}/api/users/sign-up`, user);
  }

  signIn(user: User): Observable<User> {
    return this.http.post<User>(`${environment.apiUrl}/api/users/sign-in`, user);
  }
}
