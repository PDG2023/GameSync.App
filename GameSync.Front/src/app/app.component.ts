import {Component, OnInit} from '@angular/core';
import {LoginService} from "./services/login.service";
import {AuthService} from "./services/auth.service";
import {tap} from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit{

  constructor(
    private loginService: LoginService,
    private authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.loginService
      .getMe()
      .subscribe(x => this.authService.setConnectedUser(x));
  }
}
