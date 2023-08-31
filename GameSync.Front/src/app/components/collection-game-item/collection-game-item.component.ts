import {Component} from '@angular/core';
import {GameItemComponent} from "../game-item/game-item.component";
import {ConfirmationDialogService} from "../../services/confirmation-dialog.service";
import {MessagesService} from "../../services/messages.service";
import {GamesService} from "../../services/games.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-collection-game-item',
  templateUrl: '../game-item/game-item.component.html',
  styleUrls: ['../game-item/game-item.component.scss']
})
export class CollectionGameItemComponent extends GameItemComponent {

  constructor(
    confirmationDialogService: ConfirmationDialogService,
    messagesService: MessagesService,
    gamesService: GamesService,
    router: Router
  ) {
    super(confirmationDialogService, messagesService, gamesService, router);
  }

  override navigateToDetails() {
    this.router.navigate(['/games/custom', this.game?.id])
  }
}
