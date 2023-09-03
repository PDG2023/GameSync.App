import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {map, switchMap} from 'rxjs';
import { MailConfirmationService } from '../../services/mail-confirmation.service';
import {MessagesService} from "../../services/messages.service";

@Component({
  selector: 'app-confirm-mail',
  template: ''
})
export class ConfirmMailComponent implements OnInit {
    constructor(
      private mailConfirmationService: MailConfirmationService,
      private route: ActivatedRoute,
      private messageService: MessagesService,
      private router: Router
      ) {

    }

    ngOnInit(): void {
      this.route.queryParams.pipe(
        switchMap(params => {
          let confirmationToken: string = params["confirmationToken"];
          let email: string = params["email"];

          return this.mailConfirmationService.sendConfirmationToken(confirmationToken, email)
        })
      ).subscribe(() => {
        this.messageService.success("Votre compte a été correctement activé, veuillez vous connecter");
        this.router.navigate(["/login"]);
      });
  }
}
