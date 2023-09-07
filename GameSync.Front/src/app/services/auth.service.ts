import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {Router} from "@angular/router";
import {Me} from "../models/models";
import {BehaviorSubject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  connectedUserSubject$: BehaviorSubject<Me | null> = new BehaviorSubject<Me | null>(null);
    guestUserSubject$: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  constructor(
    private router: Router
  ) {
  }

  setConnectedUser(connectedUser:  Me): void {
    this.connectedUserSubject$.next(connectedUser);
      this.guestUserSubject$.next(null);
  }

    setGuestUser(guestUser: string): void {
        this.guestUserSubject$.next(guestUser);
  }

  clearConnectedUser() {
    this.connectedUserSubject$.next(null);
  }

  signOut() {
    localStorage.removeItem(environment.securityStorage);
    this.clearConnectedUser();
    this.router.navigateByUrl('/login');
  }

}
