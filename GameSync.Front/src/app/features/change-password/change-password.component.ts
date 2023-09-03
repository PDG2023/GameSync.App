import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {LoginService} from "../../services/login.service";
import {MessagesService} from "../../services/messages.service";
import {ActivatedRoute, Router} from "@angular/router";
import {LoadingService} from "../../services/loading.service";
import {passwordMatchValidator} from "../../helpers/validators";

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  changePasswordForm: FormGroup = this.fb.nonNullable.group({
    password: ['', Validators.required],
    confirmPassword: ['', [Validators.required, passwordMatchValidator('password')]]
  });

  email: string = "";
  token: string = "";

  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private messagesService: MessagesService,
    private router: Router,
    private route: ActivatedRoute,
    protected loadingService: LoadingService
  ) {
  }


  submit(): void {
    if (!this.changePasswordForm.valid) {
      return;
    }
    console.log(this.email);
    this.loginService.changePassword(
      this.email,
      this.token,
      this.changePasswordForm.controls['password'].value,
      this.changePasswordForm.controls['confirmPassword'].value
    ).subscribe(() => {
      this.messagesService.success("Votre mot de passe a été correctement changé. Veuillez vous connecter.");
      this.router.navigate(["/login"]);
    })

  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.token = params["forgotPasswordToken"];
      this.email = params["email"];
      if (!this.token || !this.email) {
        this.messagesService.error("Le token ou le mail sont manquants");
      }

    })
  }
}
