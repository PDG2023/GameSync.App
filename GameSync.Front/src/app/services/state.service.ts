import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {User} from "../models/models";
import {LoginService} from "./login.service";

@Injectable({
  providedIn: 'root'
})
export class StateService {
  connectedUserSubject$: BehaviorSubject<User|null> = new BehaviorSubject<User|null>(null);

  constructor(
    private loginService: LoginService
  ) {
    this.loginService.me().subscribe(res => {
      this.connectedUserSubject$.next(res);
    });
  }

  setConnectedUser(connectedUser: User) : void {
    this.connectedUserSubject$.next(connectedUser);
  }

  clearConnectedUser() {
    this.connectedUserSubject$.next(null);
  }
}
