import { Injectable } from '@angular/core';
import {StateService} from "./state.service";
import {environment} from "../../environments/environment";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private stateService: StateService,
    private router: Router
  ) { }


  signOut() {
    localStorage.removeItem(environment.securityStorage);
    this.stateService.clearConnectedUser();
    this.router.navigateByUrl('/login');
  }

}
