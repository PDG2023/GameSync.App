import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";
import { LoginService } from "../../services/login.service";
import {User} from "../../models/models";
import {MessagesService} from "../../services/messages.service";
import {Router} from "@angular/router";
import {environment} from "../../../environments/environment";
import {StateService} from "../../services/state.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  constructor(
    private state: StateService,
    private fb: FormBuilder,
    private loginService: LoginService,
    private messagesService: MessagesService,
    private router: Router
  ) {
  }

  ngOnInit() {
  }

  submit(): void {
    if (this.loginForm.valid) {
      this.loginService.signIn({
        email: this.loginForm.value['email']!,
        password: this.loginForm.value['password']!,
        token: null,
        userName: null
      }).subscribe((user) => {
        localStorage.setItem(environment.securityStorage, user.token!);
        this.state.setConnectedUser(user);
        this.messagesService.success(`Bienvenu(e) ${user.userName}`);
        this.router.navigateByUrl('/');
      })
    }
    // If invalid forms, mat-error will display on click automatically.
  }
}
