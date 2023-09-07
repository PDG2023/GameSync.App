import { Component, OnInit } from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Me} from "../../models/models";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  user: Me | null = null;

  constructor(
    private authService: AuthService
  ) {

  }

  ngOnInit(): void {
    this.authService.connectedUserSubject$.subscribe(user => this.user = user);
  }
}
