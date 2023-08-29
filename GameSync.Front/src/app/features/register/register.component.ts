import {Component} from '@angular/core';
import {FormBuilder, Validators} from '@angular/forms';
import {passwordMatchValidator} from "../../helpers/validators";
import {MessagesService} from "../../services/messages.service";
import {finalize, Subject} from "rxjs";
import {Router} from "@angular/router";
import {LoginService} from "../../services/login.service";
import {LoadingService} from "../../services/loading.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
    userName: ['', Validators.required],
    confirmPassword: ['', [Validators.required, passwordMatchValidator('password')]]
  });


  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private messagesService: MessagesService,
    private router: Router,
    protected loadingService: LoadingService
  ) {
  }

  submit(): void {
    if (this.registerForm.valid) {
      this.loginService.signUp({
        email: this.registerForm.value['email']!,
        userName: this.registerForm.value['userName']!,
        password: this.registerForm.value['password']!,
      })
        .subscribe((res) => {
          this.messagesService.success('Demande envoyée avec succès ! Vérifiez vos mails pour confirmer votre inscription');
          this.router.navigateByUrl('/');
        });
    }
    // If invalid forms, mat-error will display on click automatically.
  }
}
