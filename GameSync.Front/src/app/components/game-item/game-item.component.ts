import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Router} from '@angular/router';
import {GameCollectionItem} from "../../models/models";
import {MessagesService} from "../../services/messages.service";
import {ConfirmationDialogService} from "../../services/confirmation-dialog.service";
import {GamesService} from "../../services/games.service";

@Component({
  selector: 'app-game-item',
  templateUrl: './game-item.component.html',
  styleUrls: ['./game-item.component.scss']
})
export class GameItemComponent {

  @Input() game?: GameCollectionItem;
  @Input() isReadOnly: boolean = true;

  @Output() itemDeleted = new EventEmitter();

  constructor(
    private confirmationDialogService: ConfirmationDialogService,
    private messagesService: MessagesService,
    private gamesService: GamesService,
    protected router: Router
  ) {

  }

  navigateToDetails() {
    this.router.navigate(['/games', this.game?.id]);
  }

  deleteMe() {
    this.confirmationDialogService
      .askConfirmation('Voulez-vous supprimer ce jeu de votre collection ?')
      .subscribe(res => {
        if (res) {
          this.gamesService.deleteGameFromCollection(this.game!.id, this.game!.isCustom ?? true).subscribe(() => {
            this.itemDeleted.next(null);
            this.messagesService.success('Jeu personnalisé supprimé.');
          });

        }
      });
  }


  editMe() {
    this.router.navigate(['collection/edit-game/', this.game?.id])
  }
}
