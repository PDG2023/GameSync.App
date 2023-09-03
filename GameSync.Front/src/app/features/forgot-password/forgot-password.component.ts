import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {passwordMatchValidator} from "../../helpers/validators";
import {LoadingService} from "../../services/loading.service";
import {LoginService} from "../../services/login.service";
import {MessagesService} from "../../services/messages.service";

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup = this.fb.nonNullable.group({
    email: ['',  [Validators.required, Validators.email]]
  });

  constructor(
    private fb: FormBuilder,
    protected loadingService: LoadingService,
    private loginService: LoginService,
    private messageService: MessagesService
  ) {
  }

  submit(): void {
    if (!this.forgotPasswordForm.valid) {
      return;
    }

    this.loginService
      .sendForgotPasswordRequest(this.forgotPasswordForm.get("email")?.value)
      .subscribe(() => {
        this.messageService.success("Si cet email est associé à un compte, un lien d'activation aura été envoyé sur votre boîte")
      });
  }
}
