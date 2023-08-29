import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {LoginService} from "../../services/login.service";
import {LoadingService} from "../../services/loading.service";
import {GamesService} from "../../services/games.service";
import {Router} from "@angular/router";
import {MessagesService} from "../../services/messages.service";

@Component({
  selector: 'app-add-custom-game',
  templateUrl: './add-custom-game.component.html',
  styleUrls: ['./add-custom-game.component.scss']
})
export class AddCustomGameComponent {
  gameForm: FormGroup;

  constructor(fb: FormBuilder,
              protected loadingService: LoadingService,
              private gamesService: GamesService,
              private router: Router,
              private messagesService: MessagesService
              ) {
    this.gameForm = fb.group({
      name: [null, Validators.required],
      minPlayer: [null, Validators.min(1)],
      maxPlayer: [null, [Validators.max(999), Validators.min(1)]],
      minAge: [null, Validators.min(1)],
      description: [null, Validators.required],
      durationMinute: [null, Validators.min(1)]
    });
  }

  submit(){
    this.gamesService.addGame({
      ...this.gameForm.value
    }).subscribe(() => {
      this.messagesService.success('Jeu personnalisé ajouté.');
      this.router.navigateByUrl('/collection');
    });
  }
}
