import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import {GameList} from "../../models/models";
import {MessagesService} from "../../services/messages.service";
import {ConfirmationDialogService} from "../../services/confirmation-dialog.service";

@Component({
  selector: 'app-collection-item',
  templateUrl: './collection-item.component.html',
  styleUrls: ['./collection-item.component.scss']
})
export class CollectionItemComponent {

  @Input() game?: GameList;

  constructor(
    private confirmationDialogService: ConfirmationDialogService,
    private messagesService: MessagesService,
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
          // delete
          this.messagesService.success('super, supprimé!');
        }
      });
  }


}
