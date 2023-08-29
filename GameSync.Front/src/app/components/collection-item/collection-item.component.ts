import {Component, EventEmitter, Input, Output} from '@angular/core';
import { Router } from '@angular/router';
import {Game, GameCollection} from "../../models/models";
import {MessagesService} from "../../services/messages.service";
import {ConfirmationDialogService} from "../../services/confirmation-dialog.service";
import {GamesService} from "../../services/games.service";

@Component({
  selector: 'app-collection-item',
  templateUrl: './collection-item.component.html',
  styleUrls: ['./collection-item.component.scss']
})
export class CollectionItemComponent {

  @Input() game?: Game;
  @Input() canBeDeleted: boolean = true;

  @Output() itemDeleted = new EventEmitter();

  constructor(
    private confirmationDialogService: ConfirmationDialogService,
    private messagesService: MessagesService,
    private gamesService: GamesService,
    private router: Router
  ) {

  }

  navigateToDetails() {
    this.router.navigate(['/games', this.game?.id]);
  }

  deleteMe() {
    this.confirmationDialogService
      .askConfirmation('Voulez-vous supprimer ce jeu de votre collection ?')
      .subscribe(res => {
        if(res){
          this.gamesService.deleteGameFromCollection(this.game!.id).subscribe(() => {
            this.itemDeleted.next(null);
            this.messagesService.success('super, supprimé!');
          });

        }
      });
  }


}
