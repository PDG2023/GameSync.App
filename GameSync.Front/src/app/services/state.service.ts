import {Injectable} from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {User} from "../models/models";

@Injectable({
  providedIn: 'root'
})
export class StateService {
  connectedUserSubject$: BehaviorSubject<User | null> = new BehaviorSubject<User | null>(null);

  constructor() {
  }

  setConnectedUser(connectedUser: User) : void {
    this.connectedUserSubject$.next(connectedUser);
  }

  clearConnectedUser() {
    this.connectedUserSubject$.next(null);
  }
}
