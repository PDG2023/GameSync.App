import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {LoginService} from "../../services/login.service";
import {MessagesService} from "../../services/messages.service";
import {Router} from "@angular/router";
import {environment} from "../../../environments/environment";
import {AuthService} from "../../services/auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  constructor(
      private authService: AuthService,
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
        ...this.loginForm.value
      }).subscribe((user) => {
        localStorage.setItem(environment.securityStorage, user.token!);
        this.authService.setConnectedUser(user);
        this.messagesService.success(`Bienvenu(e) ${user.userName}`);
        this.router.navigateByUrl('/');
      })
    }
    // If invalid forms, mat-error will display on click automatically.
  }
}
