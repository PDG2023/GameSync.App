import { Injectable } from '@angular/core';
import {environment} from "../../environments/environment";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  connectedUserSubject$: BehaviorSubject<User | null> = new BehaviorSubject<User | null>(null);

  constructor(
    private router: Router
  ) {
  }

  setConnectedUser(connectedUser: User): void {
    this.connectedUserSubject$.next(connectedUser);
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
