import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Party} from "../../models/models";
import {ConfirmationDialogService} from "../../services/confirmation-dialog.service";
import {MessagesService} from "../../services/messages.service";
import {PartiesService} from "../../services/parties.service";

@Component({
  selector: 'app-party-item',
  templateUrl: './party-item.component.html',
  styleUrls: ['./party-item.component.scss']
})
export class PartyItemComponent {
  @Input() party?: Party;

  @Output() partyRemoved = new EventEmitter();

  constructor(
    private confirmationService: ConfirmationDialogService,
    private messagesService: MessagesService,
    private partiesService: PartiesService
  ) {
  }



  removeParty() {
    this.confirmationService.askConfirmation('Voulez-vous supprimer la rée-soi ?').subscribe(res => {
      if(res) {
        this.partiesService.deleteParty(this.party!.id).subscribe(() => {
          this.partyRemoved.next(null);
          this.messagesService.success('Rée-soi supprimée, BRAVO');
        })
      }
    });
}


}
