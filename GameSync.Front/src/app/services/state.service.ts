import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {User} from "../models/models";

@Injectable({
  providedIn: 'root'
})
export class StateService {
  private connectedUserSubject$: BehaviorSubject<User> | null = null;
  connectedUser$: Observable<User> | undefined = this.connectedUserSubject$?.asObservable();

  constructor() {
  }

  setConnectedUser(connectedUser: User) : void {
    this.connectedUserSubject$?.next(connectedUser);
  }
}
